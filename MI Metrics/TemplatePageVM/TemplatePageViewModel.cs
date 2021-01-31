using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class TemplatePageViewModel
    {
        private TemplatePage templatePage;
        public ManageControlFileUtil controlFileUtil;
        private TemplateChange templateChange;
        private bool changesAreRecording;

        // ProductView will reference the ProductView in the ProductManager.
        public ObservableCollection<Product> ProductView { get => productView; set => productView = value; }
        private ObservableCollection<Product> productView;


        public ObservableCollection<Test> TestView { get => testView; set => testView = value; }
        private ObservableCollection<Test> testView;

        public ObservableCollection<ControlFile> ControlFileView { get => controlFileView; set => controlFileView = value; }
        private ObservableCollection<ControlFile> controlFileView;

        public bool ChangesAreRecording { get => changesAreRecording; set => changesAreRecording = value; }


        public TemplatePageViewModel(TemplatePage templatePage)
        {
            this.templatePage = templatePage;
            templatePage.DataContext = this;
            controlFileUtil = new ManageControlFileUtil(this);

            TestView = new ObservableCollection<Test>();
            controlFileView = new ObservableCollection<ControlFile>();
            productView = MainWindow.productManager.ProductView;
            
            ChangesAreRecording = false;
        }

        public void BuildViewedTestList(Product product)
        {
            TestView.Clear();
            foreach(Test test in product.Tests)
            {
                TestView.Add(test.Copy());
            }
        }
        public void ClearTestList()
        {
            TestView.Clear();
        }

        public void BuildViewedControlFileList(Product product)
        {
            ControlFileView.Clear();

            List<ControlFile> controlFiles = MainWindow.database.controlFileManager.RetrieveControlFileList(product);
            foreach(ControlFile controlFile in controlFiles)
            {
                ControlFileView.Add(controlFile.Copy());
            }
        }

        public void ClearControlFileList()
        {
            ControlFileView.Clear();
        }

        public void AddTemporaryTestStep(string step, string mStep, string subStep, string lowerLimitString, string upperLimitString, Test test)
        {
            // This will temporarily store the created test step in the TestSteps and TestStepView lists of the passed in test.
            double? lowerLimit = null;
            if(!(lowerLimitString == null || lowerLimitString.Trim().Equals("")))
            {
                double dummy;
                if (Double.TryParse(lowerLimitString, out dummy)) lowerLimit = dummy;
            }
            double? upperLimit = null;
            if (!(upperLimitString == null || upperLimitString.Trim().Equals("")))
            {
                double dummy;
                if (Double.TryParse(upperLimitString, out dummy)) upperLimit = dummy;
            }

            if (subStep == null) subStep = "";

            TestStep testStep = new TestStep(step, mStep, subStep, lowerLimit, upperLimit, test.TestID);
            test.TestStepView.Add(testStep);
            if (ChangesAreRecording)
            {
                templateChange.AddTestStep(testStep);
            }
        }

        public void AddAllTestSteps(ControlFile controlFile)
        {
            List<Test> tests = new List<Test>(testView);

            foreach(Test test in tests)
            {
                foreach(TestStep testStep in test.TestStepView)
                {
                    testStep.ControlFileID = controlFile.ControlFileID;
                    MainWindow.database.testStepManager.AddTestStep(testStep);
                }
            }
        }

        public void AssignTestStepsToTests(ControlFile controlFile)
        {
            foreach (TestStep testStep in controlFile.TestSteps)
            {
                foreach(Test test in TestView)
                {
                    if(test.TestID == testStep.TestID)
                    {
                        test.TestStepView.Add(testStep);
                        break;
                    }
                }
            }
        }

        public void RecordTemplateChangesStart()
        {
            templateChange = new TemplateChange();
            ChangesAreRecording = true;
        }

        public void RecordTemplateChangeStop()
        {
            templateChange = null;
            ChangesAreRecording = false;
        }

        public void DeleteTestStepFromTest(Test test, TestStep testStep)
        {
            if (ChangesAreRecording)
            {
                templateChange.DeleteTestStep(testStep);
            }
            test.TestStepView.Remove(testStep);
        }

        public void EditTemporaryTestStep(string step, string mStep, string subStep, string lowerLimitString, string upperLimitString, TestStep testStep)
        {
            double? lowerLimit = null;
            if (!(lowerLimitString == null || lowerLimitString.Trim().Equals("")))
            {
                double dummy;
                if (Double.TryParse(lowerLimitString, out dummy)) lowerLimit = dummy;
            }
            double? upperLimit = null;
            if (!(upperLimitString == null || upperLimitString.Trim().Equals("")))
            {
                double dummy;
                if (Double.TryParse(upperLimitString, out dummy)) upperLimit = dummy;
            }

            testStep.Step = step;
            testStep.MStep = mStep;
            testStep.LowerLimit = lowerLimit;
            testStep.UpperLimit = upperLimit;

            if (subStep == null) subStep = "";
            testStep.SubStep = subStep;

            if (ChangesAreRecording)
            {
                templateChange.EditTestStep(testStep);
            }
        }

        public bool EditTemplate(ControlFile controlFile, string numberText, string type, string part, string version)
        {
            long number;
            if (!Int64.TryParse(numberText, out number)) return false;
            if (!DatabaseHelper.ValidateCharacters(type) || !DatabaseHelper.ValidateCharacters(part) || !DatabaseHelper.ValidateCharacters(version))
            {
                return false;
            }

            foreach(TestStep testStep in templateChange.addedTestSteps)
            {
                testStep.ControlFileID = controlFile.ControlFileID;
            }

            controlFileUtil.EditControlFile(controlFile, number, type, part, version);
            controlFileUtil.EditTestSteps(templateChange);

            
            return true;
        }
    }
}
