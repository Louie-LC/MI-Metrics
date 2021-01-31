using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class DatabaseProductManager
    {
        
        public Product AddProduct(string name, string imageLocation)
        {
            int id = Database.GetNextID(Table.Products);
            Product product = new Product(id, name, imageLocation);
            var p = new DynamicParameters();
            p.Add("@ProductID", id);
            p.Add("@Name", name);
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                if (imageLocation == null || imageLocation.Equals(""))
                {
                    p.Add("@Image", null);
                }
                else
                {
                    p.Add("@Image", imageLocation);
                }
                connection.Execute("dbo.spInsertProduct", p, commandType: CommandType.StoredProcedure);

            }
            return product;
        }

        public Product GetProduct(int productId)
        {
            List<Product> products = new List<Product>();
            var p = new DynamicParameters();
            p.Add("@ProductID", productId);
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                products = connection.Query<Product>("dbo.spSelectProduct", p, commandType: CommandType.StoredProcedure).ToList();
            }
            if (products.Count > 0)
            {
                return products[0];
            }
            return null;
        }

        public List<Product> RetrieveProductList()
        {
            List<Product> products = new List<Product>();
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                products = connection.Query<Product>("dbo.spSelectAllProducts").ToList();
            }
            return products;
        }

        public void UpdateProduct(int id, string name, string imageLocation)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ProductID", id);
                p.Add("@Name", name);
                connection.Open();
                if (imageLocation == null || imageLocation.Equals(""))
                {
                    p.Add("@Image", null);
                    //connection.Execute("dbo.spUpdateProduct '" + id + "', '" + name + "', 'NULL'");
                }
                else
                {
                    p.Add("@Image", imageLocation);
                    //connection.Execute("dbo.spUpdateProduct '" + id + "', '" + name + "', '" + imageLocation + "'");
                }
                connection.Execute("dbo.spUpdateProduct", p, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
