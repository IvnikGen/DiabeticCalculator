using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnector.Methods
{
    public class Create
    {
        public static string exception { get; set; }

        public static bool insertProduct(Products product)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            bool result = false;

            string query = @"INSERT INTO [dbo].[BreadUnitsTable]
           ([Product],[ProductGroup],[Carbohydrates],[GrammInUnit],[BreadUnits])
            VALUES
           (@Product,@ProductGroup,@Carbohydrates,@GrammInUnit,@BreadUnits)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            cmd.Parameters.Add("@Product", SqlDbType.NVarChar).Value = product.Product;
            cmd.Parameters.Add("@ProductGroup", SqlDbType.Int).Value = product.ProductGroup;
            cmd.Parameters.Add("@Carbohydrates", SqlDbType.Float).Value = product.Carbohydrates;
            cmd.Parameters.Add("@GrammInUnit", SqlDbType.Float).Value = product.GrammInUnit;
            cmd.Parameters.Add("@BreadUnits", SqlDbType.Float).Value = product.BreadUnits;

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
                exception = e.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    SqlConnection.ClearPool(connection);
                    connection.Dispose();
                }
            }

            return result;
        }

        public static bool insertProductGroups(ProductGroups productGroups)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            bool result = false;

            string query = @"INSERT INTO [dbo].[Groups]
           ([Name],[Image])
            VALUES
           (@Name,@Image)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = productGroups.GroupName;
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = productGroups.GroupImage;

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
                exception = e.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    SqlConnection.ClearPool(connection);
                    connection.Dispose();
                }
            }

            return result;
        }

        public static int insertAreaFirst(string userID)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            int result = -1;

            string query = @"INSERT INTO [dbo].[PersonalArea]
           ([UserID]) output INSERTED.ID
            VALUES
           (@UserID)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;

            try
            {
                connection.Open();
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                exception = e.Message;
                return -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    SqlConnection.ClearPool(connection);
                    connection.Dispose();
                }
            }

            return result;
        }

        public static bool insertArea(PersonalArea Area)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            bool result = false;

            string query = @"INSERT INTO [dbo].[PersonalArea]
           ([UserID],[Product],[ProductGroup],[Carbohydrates],[GrammInUnit],[BreadUnits],[DateCreate], [RecipeID] )
            VALUES
           (@UserID, @Product,@ProductGroup,@Carbohydrates,@GrammInUnit,@BreadUnits,@DateCreate, @RecipeID)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Area.ID;
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = Area.UserID;
            cmd.Parameters.Add("@Product", SqlDbType.NVarChar).Value = Area.Product;
            cmd.Parameters.Add("@ProductGroup", SqlDbType.NVarChar).Value = Area.ProductGroupName;
            cmd.Parameters.Add("@Carbohydrates", SqlDbType.Float).Value = Area.Carbohydrates;
            cmd.Parameters.Add("@GrammInUnit", SqlDbType.Float).Value = Area.GrammInUnit;
            cmd.Parameters.Add("@BreadUnits", SqlDbType.Float).Value = Area.BreadUnits;
            cmd.Parameters.Add("@DateCreate", SqlDbType.DateTime).Value = Area.DateCreate;
            cmd.Parameters.Add("@RecipeID", SqlDbType.Float).Value = Area.RecipeID;

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
                exception = e.Message;
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    SqlConnection.ClearPool(connection);
                    connection.Dispose();
                }
            }

            return result;
        }

        public static int insertJournal(Journal Journal)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            int result = -1;

            string query = @"INSERT INTO [dbo].[Recipe]
           ([Title],[Created], [Insulin], [SugarLevel] )
            output INSERTED.ID VALUES
           (@Title, @Created, @Insulin, @SugarLevel)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Journal.ID;
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Journal.Title;
            cmd.Parameters.Add("@Created", SqlDbType.DateTime).Value = Journal.Created;
            cmd.Parameters.Add("@Insulin", SqlDbType.Float).Value = Journal.Insulin;
            cmd.Parameters.Add("@SugarLevel", SqlDbType.Float).Value = Journal.SugarLevel;
            

            try
            {
                connection.Open();
                int modified = (int)cmd.ExecuteScalar();
                result = modified;
            }
            catch (Exception e)
            {
                exception = e.Message;
                return result;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    SqlConnection.ClearPool(connection);
                    connection.Dispose();
                }
            }

            return result;
        }
    }
}
