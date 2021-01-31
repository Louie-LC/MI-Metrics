using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MI_Metrics
{
    public class DatabaseTestManager
    {
        public List<Test> RetrieveTests(Product product)
        {
            var p = new DynamicParameters();
            p.Add("@ProductID", product.ProductID);
            List<Test> tests = new List<Test>();

            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                tests = connection.Query<Test>("dbo.spSelectTestWithProductID", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return tests;
        }

        public Test AddTest(string name, Product product)
        {
            int id = Database.GetNextID(Table.Tests);
            Test test = new Test(id, name, product.ProductID);
            var p = new DynamicParameters();
            p.Add("@TestID", test.TestID);
            p.Add("@Name", test.Name);
            p.Add("ProductID", test.ProductID);

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                connection.Execute("dbo.spInsertTest", p, commandType: CommandType.StoredProcedure);
            }
            return test;
        }
        
        public void UpdateTest(int testID, string name)
        {
            var p = new DynamicParameters();
            p.Add("@TestID", testID);
            p.Add("@Name", name);

            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                connection.Execute("dbo.spUpdateTest", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
