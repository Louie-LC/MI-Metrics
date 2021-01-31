using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class TestResult
    {
        private long testResultID;
        private int testID;
        private double value;
        private DateTime date;
        private int deviceHistoryRecordID;

        public long TestResultID { get => testResultID; set => testResultID = value; }
        public int TestID { get => testID; set => testID = value; }
        public double Value { get => value; set => this.value = value; }
        public DateTime Date { get => date; set => date = value; }
        public int DeviceHistoryRecordID { get => deviceHistoryRecordID; set => deviceHistoryRecordID = value; }

        public TestResult(long testResultID, double value, DateTime date, int testID, int deviceHistoryRecordID)
        {
            TestResultID = testResultID;
            TestID = testID;
            Value = value;
            Date = date.Date;
            DeviceHistoryRecordID = deviceHistoryRecordID;
        }

        public TestResult(int testID, double value, DateTime date)
        {
            TestResultID = testResultID;
            TestID = testID;
            Value = value;
            Date = date.Date;
            DeviceHistoryRecordID = deviceHistoryRecordID;
        }
    }
}
