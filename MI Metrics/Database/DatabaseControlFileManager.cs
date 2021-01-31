using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MI_Metrics
{
    public class DatabaseControlFileManager
    {
        public List<ControlFile> RetrieveControlFileList(Product product)
        {
            List<ControlFile> controlFiles = new List<ControlFile>();
            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ProductID", product.ProductID);
                connection.Open();
                controlFiles = connection.Query<ControlFile>("dbo.spSelectControlFileWithProductID", p, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            return controlFiles;
        }

        public ControlFile AddControlFile(long number, string type, string part, string version, Product product)
        {
            int controlFileID = Database.GetNextID(Table.ControlFiles);
            ControlFile controlFile = new ControlFile(controlFileID, number, type, part, version, product.ProductID);
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ControlFileID", controlFile.ControlFileID);
                p.Add("@Number", controlFile.Number);
                p.Add("@Type", controlFile.Type);
                p.Add("@Part", controlFile.Part);
                p.Add("@Version", controlFile.Version);
                p.Add("@ProductID", controlFile.ProductID);

                connection.Open();
                connection.Execute("dbo.spInsertControlFile", p, commandType: CommandType.StoredProcedure);
            }
            return controlFile;
        }

        public void EditControlFile(ControlFile controlFile, long number, string type, string part, string version)
        {
            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ControlFileID", controlFile.ControlFileID);
                p.Add("@Number", number);
                p.Add("@Type", type);
                p.Add("@Part", part);
                p.Add("@Version", version);

                connection.Open();
                connection.Execute("dbo.spUpdateControlFile", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
