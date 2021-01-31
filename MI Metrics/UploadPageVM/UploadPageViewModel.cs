using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class UploadPageViewModel : INotifyPropertyChanged
    {
        private UploadPage uploadPage;
        private PullDataUtil pullDataUtil;
        private ParseDataUtil parseDataUtil;

        public ObservableCollection<Product> ProductView { get => productView; set => productView = value; }
        private ObservableCollection<Product> productView;

        // A CollectionViewSource is created in the XAML file for displaying test results. testReulsts is the container that the view binds to.
        private Results testResults;

        public string SelectedFileName
        {
            get => selectedFileName;
            set
            {
                selectedFileName = value;
                OnPropertyChanged();
            }
        }
        string selectedFileName;
        string selectedFileFullPath;

        private IntermediateTestResult selectedTestResult;

        public int ErrorCount
        {
            get => errorCount;
            set
            {
                errorCount = value;
                OnPropertyChanged();
            }
        }
        private int errorCount;
        public int WarningCount
        {
            get => warningCount;
            set
            {
                warningCount = value;
                OnPropertyChanged();
            }
        }
        private int warningCount;

        public int TotalCount
        {
            get => totalCount;
            set
            {
                totalCount = value;
                OnPropertyChanged();
            }
        }
        private int totalCount;

        public UploadPageViewModel(UploadPage uploadPage, Results results)
        {
            this.uploadPage = uploadPage;
            uploadPage.DataContext = this;

            pullDataUtil = new PullDataUtil();

            selectedFileName = "";
            selectedFileFullPath = "";
            BuildProductView();

            ProductView = MainWindow.productManager.ProductView;

            ErrorCount = 0;
            WarningCount = 0;
            testResults = results;
        }

        private void BuildProductView()
        {
            productView = new ObservableCollection<Product>(MainWindow.productManager.Products);
        }

        public SelectFileResult SelectCSVFile()
        {
            SelectFileResult result = DialogManager.ShowDialog(DialogType.CSV);
            if (result.validFileSelected)
            {
                selectedFileFullPath = result.filePath;
                SelectedFileName = Path.GetFileName(selectedFileFullPath);
                result.validFileSelected = ValidateFile(selectedFileFullPath);
            }
            return result;
        }

        private bool ValidateFile(string filePath)
        {
            String fileName = Path.GetFileName(filePath);
            if (!File.Exists(filePath) || fileName.Equals("") || fileName == null)
            {
                return false;
            }
            return true;
        }

        public void ClearSelectedFile()
        {
            SelectedFileName = "";
            selectedFileFullPath = "";
        }

        public bool PullData(Product product)
        {
            parseDataUtil = new ParseDataUtil();

            testResults.Clear();
            if (!File.Exists(selectedFileFullPath)) return false;
            List<IntermediateTestResult> tempTestResults = pullDataUtil.PullDataFromCSV(product, selectedFileFullPath);

            foreach (IntermediateTestResult result in tempTestResults)
            {
                testResults.Add(result);
                parseDataUtil.ValidateTest(result, product);
            }
            FindAllWarningsandErrors();
            TotalCount = testResults.Count;
            ClearSelectedFile();
            return true;
        }

        private void FindAllWarningsandErrors()
        {
            WarningCount = 0;
            ErrorCount = 0;
            foreach (IntermediateTestResult result in testResults)
            {
                if (result.WarningFound)
                    WarningCount++;
                if (result.ErrorFound)
                    ErrorCount++;
            }
        }

        public void IntermediateTestResultSelected(IntermediateTestResult result)
        {
            selectedTestResult = result;
        }

        public void DeleteSelectedTestResult()
        {
            if (selectedTestResult != null)
            {
                DeleteTestResult(selectedTestResult);
            }
        }

        public void DeleteTestResult(IntermediateTestResult result)
        {
            testResults.Remove(result);
            if (result.ErrorFound)
                ErrorCount--;
            if (result.WarningFound)
                WarningCount--;
            TotalCount--;

            // This will check if any CSV collisions are occuring still as a result of deleting result.
            // If result was colliding with any other IntermediateTestResult, RemoveTestAndCheckForCollisions
            // will return the other result if it ends up having no remaining collisions so that it too can be
            // removed as an error.
            IntermediateTestResult otherResult = parseDataUtil.RemoveTestAndCheckForCollisions(result);
            if (otherResult != null && !otherResult.ErrorFound)
            {
                ErrorCount--;
            }
            selectedTestResult = null;
        }

        public int UploadTestResults()
        {
            int errorCount = 0;
            foreach (IntermediateTestResult intermediateResult in testResults)
            {
                TestResult result = CreateTest(intermediateResult);
                if (result != null)
                {
                    DeviceHistoryRecord record = StoreandRetrieveDeviceHistoryRecord(intermediateResult);
                    result.DeviceHistoryRecordID = record.DeviceHistoryRecordID;
                    if (intermediateResult.WarningFound)
                    {
                        UpdateTestResult(result, record);
                    }
                    else
                    {
                        StoreTestResult(result);
                    }
                }
                else
                {
                    errorCount++;
                }
            }
            return errorCount;
        }

        private DeviceHistoryRecord StoreandRetrieveDeviceHistoryRecord(IntermediateTestResult intermediateResult)
        {
            DeviceHistoryRecord record;
            if (parseDataUtil.existingDeviceHistoryRecords.ContainsKey(intermediateResult.SystemDescription))
            {
                record = parseDataUtil.existingDeviceHistoryRecords[intermediateResult.SystemDescription];
            }
            else
            {
                record = parseDataUtil.newDeviceHistoryRecords[intermediateResult.SystemDescription];
                MainWindow.database.deviceHistoryRecordManager.AddDeviceHistoryRecord(record);
                parseDataUtil.existingDeviceHistoryRecords.Add(record.FullDescription, record);
                parseDataUtil.newDeviceHistoryRecords.Remove(record.FullDescription);
            }
            return record;
        }

        private void StoreTestResult(TestResult result)
        {
            MainWindow.database.testResultManager.AddTestResult(result);
        }

        private void UpdateTestResult(TestResult result, DeviceHistoryRecord record)
        {
            // The test going to be updated should be in the TestResults dictionary.
            // Because it was pulled from the database, it should have a TestResultID and
            // that is what will be used to update the correct row in the datbase.
            result.TestResultID = record.TestResults[result.TestID].TestResultID;
            MainWindow.database.testResultManager.UpdateTestResult(result);
        }

        public TestResult CreateTest(IntermediateTestResult intermediateResult)
        {
            double value;
            DateTime date;
            if (!Double.TryParse(intermediateResult.Value, out value) || !DateTime.TryParse(intermediateResult.Date, out date))
            {
                return null;
            }
            return new TestResult(intermediateResult.TestID, value, date);
        }

        public void ClearTestResults()
        {
            parseDataUtil = null;
            testResults.Clear();

            ErrorCount = 0;
            WarningCount = 0;
            TotalCount = 0;

            selectedFileName = "";
            selectedFileFullPath = "";
            selectedTestResult = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
