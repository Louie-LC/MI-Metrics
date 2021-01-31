using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace MI_Metrics
{
    public class DatabaseTestResultManager
    {
        public List<TestResult> GetTestResults(DeviceHistoryRecord record)
        {
            List<TestResult> results = null;

            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@DeviceHistoryRecordID", record.DeviceHistoryRecordID);

                connection.Open();
                results = connection.Query<TestResult>("dbo.spSelectTestResultWithDHRID", p, commandType: CommandType.StoredProcedure).ToList();
            }

            return results;
        }

        public void AddTestResult(TestResult result)
        {
            result.TestResultID = Database.GetNextID(Table.TestResults);

            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TestResultID", result.TestResultID);
                p.Add("@Value", result.Value);
                p.Add("@Date", result.Date);
                p.Add("@TestID", result.TestID);
                p.Add("@DeviceHistoryRecordID", result.DeviceHistoryRecordID);

                connection.Open();
                connection.Execute("dbo.spInsertTestResult", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateTestResult(TestResult result)
        {
            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TestResultID", result.TestResultID);
                p.Add("@Value", result.Value);
                p.Add("@Date", result.Date);

                connection.Open();
                connection.Execute("dbo.spUpdateTestResult", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
