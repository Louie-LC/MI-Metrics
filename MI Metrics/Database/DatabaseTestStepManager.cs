using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace MI_Metrics
{
    public class DatabaseTestStepManager
    {
        public void AddTestStep(TestStep testStep)
        {
            int testStepID = Database.GetNextID(Table.TestSteps);
            testStep.TestStepID = testStepID;

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TestStepID", testStep.TestStepID);
                p.Add("@Step", testStep.Step);
                p.Add("@MStep", testStep.MStep);
                p.Add("@SubStep", testStep.SubStep);
                p.Add("@LowerLimit", testStep.LowerLimit);
                p.Add("@UpperLimit", testStep.UpperLimit);
                p.Add("@ControlFileID", testStep.ControlFileID);
                p.Add("@TestID", testStep.TestID);

                connection.Open();
                connection.Execute("dbo.spInsertTestStep", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditTestStep(TestStep testStep)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TestStepID", testStep.TestStepID);
                p.Add("@Step", testStep.Step);
                p.Add("@MStep", testStep.MStep);
                p.Add("@SubStep", testStep.SubStep);
                p.Add("@LowerLimit", testStep.LowerLimit);
                p.Add("@UpperLimit", testStep.UpperLimit);

                connection.Open();
                connection.Execute("dbo.spUpdateTestStep", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteTestStep(TestStep testStep)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TestStepID", testStep.TestStepID);

                connection.Open();
                connection.Execute("dbo.spDeleteTestStep", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteTestStepsFromControlFile(ControlFile controlFile)
        {
            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ControlFileID", controlFile.ControlFileID);

                connection.Open();
                connection.Execute("dbo.spDeleteTestStepsUsingControlFileID", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<TestStep> GetTestStepList(ControlFile controlFile)
        {
            List<TestStep> testSteps = null;

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ControlFileID", controlFile.ControlFileID);
                testSteps = connection.Query<TestStep>("dbo.spSelectTestStepWithControlFileID", p, commandType: CommandType.StoredProcedure).ToList();
            }

            return testSteps;
        }
    }
}
