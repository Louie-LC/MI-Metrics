using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MI_Metrics
{
    public class DatabaseDeviceHistoryRecordManager
    {

        public List<DeviceHistoryRecord> GetDeviceHistoryRecords(long partNumber, string type, string part, string version, long serialNumber)
        {
            List<DeviceHistoryRecord> records = null;

            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@PartNumber", partNumber);
                p.Add("@Type", type);
                p.Add("@Part", part);
                p.Add("@Version", version);
                p.Add("@SerialNumber", serialNumber);

                connection.Open();
                records = connection.Query<DeviceHistoryRecord>("dbo.spSelectDeviceHistoryRecordsWithDescription", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return records;
        }

        public List<DeviceHistoryRecord> GetDeviceHistoryRecords(Product product)
        {
            List<DeviceHistoryRecord> records = null;

            using(var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ProductID",product.ProductID);

                connection.Open();
                records = connection.Query<DeviceHistoryRecord>("dbo.spSelectDeviceHistoryRecordsWithProductID", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return records;
        }

        public void AddDeviceHistoryRecord(DeviceHistoryRecord record)
        {
            record.DeviceHistoryRecordID = Database.GetNextID(Table.DeviceHistoryRecords);

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@DeviceHistoryRecordID", record.DeviceHistoryRecordID);
                p.Add("@PartNumber", record.PartNumber);
                p.Add("@Type", record.Type);
                p.Add("@Part", record.Part);
                p.Add("@Version", record.Version);
                p.Add("@SerialNumber", record.SerialNumber);
                p.Add("@ProductID", record.ProductID);

                connection.Open();
                connection.Execute("dbo.spInsertDeviceHistoryRecord", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
