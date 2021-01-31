using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace MI_Metrics
{
    public class DatabaseTagManager
    {
        public Tag AddTag(string description, Product product)
        {
            int tagID = Database.GetNextID(Table.Tag);
            Tag tag = new Tag(tagID, description, product.ProductID);

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TagID", tag.TagID);
                p.Add("@Description", tag.Description);
                p.Add("@ProductID", tag.ProductID);

                connection.Open();
                connection.Execute("dbo.spInsertTag", p, commandType: CommandType.StoredProcedure);
            }

            return tag;
        }

        public List<Tag> GetTags(Product product)
        {
            List<Tag> tags = null;

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ProductID", product.ProductID);

                connection.Open();
                tags = connection.Query<Tag>("dbo.spSelectTagWithProductID", p, commandType: CommandType.StoredProcedure).ToList();
            }

            return tags;
        }

        public void UpdateTag(Tag tag, string description)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TagID", tag.TagID);
                p.Add("@Description", description);

                connection.Open();
                connection.Execute("dbo.spUpdateTag", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<TagUnitLink> GetTagUnitLinks(Unit unit)
        {
            List<TagUnitLink> tagUnitLinks = null;

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@UnitID", unit.UnitID);

                connection.Open();
                tagUnitLinks = connection.Query<TagUnitLink>("dbo.spSelectTagUnitLinkWithUnitID", p, commandType: CommandType.StoredProcedure).ToList();
            }

            return tagUnitLinks;
        }

        public List<TagUnitLink> GetTagUnitLinks(Tag tag)
        {
            List<TagUnitLink> tagUnitLinks = null;

            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TagID", tag.TagID);

                connection.Open();
                tagUnitLinks = connection.Query<TagUnitLink>("dbo.spSelectTagUnitLinkWithTagID", p, commandType: CommandType.StoredProcedure).ToList();
            }

            return tagUnitLinks;
        }

        public void AddTagUnitLink(Tag tag, Unit unit)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TagID", tag.TagID);
                p.Add("@UnitID", unit.UnitID);

                connection.Open();
                List<TagUnitLink> links = connection.Query<TagUnitLink>("dbo.spSelectTagUnitLink", p, commandType: CommandType.StoredProcedure).ToList();
                if (links.Count == 0)
                {
                    connection.Execute("dbo.spInsertTagUnitLink", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public void DeleteTagUnitLink(Tag tag, Unit unit)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TagID", tag.TagID);
                p.Add("@UnitID", unit.UnitID);

                connection.Open();
                connection.Execute("dbo.spDeleteTagUnitLink", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
