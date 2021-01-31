using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class DatabaseUnitManager
    {
        public List<Unit> RetrieveUnitList(Product product)
        {
            List<Unit> units = new List<Unit>();
            var p = new DynamicParameters();
            p.Add("@ProductID", product.ProductID);
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                units = connection.Query<Unit>("dbo.spSelectUnitWithProductID", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return units;
        }

        public List<Unit> RetrieveUnitList(int unitID)
        {
            List<Unit> units = new List<Unit>();
            var p = new DynamicParameters();
            p.Add("@UnitID", unitID);

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                units = connection.Query<Unit>("dbo.spSelectUnitWithUnitID", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return units;
        }

        public Unit AddUnit(long partNumber, string description, Product product)
        {
            int id = Database.GetNextID(Table.Units);
            Unit unit = new Unit(id, partNumber, description, product.ProductID);
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@UnitID", id);
                p.Add("@PartNumber", partNumber);
                p.Add("@Description", description);
                p.Add("@ProductID", product.ProductID);

                connection.Open();
                connection.Execute("dbo.spInsertUnit", p, commandType: CommandType.StoredProcedure);
            }
            return unit;
        }

        public void UpdateUnit(Unit unit, long partNumber, string description)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@UnitID", unit.UnitID);
                p.Add("@PartNumber", partNumber);
                p.Add("@Description", description);

                connection.Open();
                connection.Execute("dbo.spUpdateUnit", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
