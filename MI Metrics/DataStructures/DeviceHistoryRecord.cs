using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class DeviceHistoryRecord
    {
        private int deviceHistoryRecordID;
        private long partNumber;
        private long serialNumber;
        private string type;
        private string part;
        private string version;
        private string fullDescription;
        private int productID;
        private DateTime date;
        private Dictionary<int, TestResult> testResults;
        private dynamic testValues;
        private string unitDescription;

        public int DeviceHistoryRecordID { get => deviceHistoryRecordID; set => deviceHistoryRecordID = value; }
        public long PartNumber { get => partNumber; set => partNumber = value; }
        public long SerialNumber { get => serialNumber; set => serialNumber = value; }
        public string Type { get => type; set => type = value; }
        public string Part { get => part; set => part = value; }
        public string Version { get => version; set => version = value; }
        public string FullDescription { get => fullDescription; set => fullDescription = value; }
        public int ProductID { get => productID; set => productID = value; }
        public DateTime Date { get => date; set => date = value; }
        public Dictionary<int, TestResult> TestResults { get => testResults; set => testResults = value; }
        public dynamic TestValues { get => testValues; set => testValues = value; }
        public string UnitDescription { get => unitDescription; set => unitDescription = value; }

        public DeviceHistoryRecord(int deviceHistoryRecordID, long partNumber, string type, string part, string version, long serialNumber, int productID)
        {
            DeviceHistoryRecordID = deviceHistoryRecordID;
            PartNumber = partNumber;
            Type = type;
            Part = part;
            Version = version;
            SerialNumber = serialNumber;
            ProductID = productID;
            BuildFullDesciprtion();
        }

        public DeviceHistoryRecord(long partNumber, string type, string part, string version, long serialNumber, int productID)
        {
            PartNumber = partNumber;
            Type = type;
            Part = part;
            Version = version;
            SerialNumber = serialNumber;
            ProductID = productID;
            TestResults = new Dictionary<int, TestResult>();
            BuildFullDesciprtion();
        }

        public void BuildFullDesciprtion()
        {
            fullDescription = String.Format("{0}_{1}_{2}_{3}_{4}", PartNumber, Type, Part, Version, SerialNumber);
        }

        public void BuildTestResultsAndDate()
        {
            // This ensures that BuildTestResults only happens once for each DHR.
            if(TestResults == null)
            {
                DateTime recentDate = DateTime.MinValue;
                TestResults = new Dictionary<int, TestResult>();
                
                List<TestResult> results = MainWindow.database.testResultManager.GetTestResults(this);
                foreach (TestResult result in results)
                {
                    if (!TestResults.ContainsKey(result.TestID))
                    {
                        TestResults.Add(result.TestID, result);
                    }
                    if(result.Date > recentDate)
                    {
                        recentDate = result.Date;
                    }
                }
                Date = recentDate;
            }
        }

        public void BuildTestValues(Product product)
        {
            // Each DHR needs test values built for it so that the UI can bind to them.
            // TestValue needs is a dynamic object that will have a property for each test that
            // is being trakced int he associated product.
            var dyn = new ExpandoObject() as IDictionary<string, object>;

            foreach (var test in product.Tests)
            {
                double? testValue = null;
                if (TestResults.ContainsKey(test.TestID))
                    testValue = TestResults[test.TestID].Value;

                string name = test.Name.Replace(" ", "");
                dyn.Add(name, testValue);
            }

            TestValues = dyn;
        }

        public void BuildUnitDescription(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                if (unit.PartNumber == partNumber)
                {
                    unitDescription = unit.Description;
                    return;
                }
            }
        }
    }
}
