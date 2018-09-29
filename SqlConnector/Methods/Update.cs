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
    public class Update
    {
        public static string exception { get; set; }

        static public bool updateBreadUnitsTable(Products product)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            string query = "UPDATE [dbo].[BreadUnitsTable] " +
                "SET [Product] = @Product," +
                "[ProductGroup] = @ProductGroup," +
                "[Carbohydrates] = @Carbohydrates," +
                "[GrammInUnit] = @GrammInUnit," +
                "[BreadUnits] = @BreadUnits " +
                "WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@Product", SqlDbType.NVarChar).Value = product.Product;
            cmd.Parameters.Add("@ProductGroup", SqlDbType.Int).Value = product.ProductGroup;
            cmd.Parameters.Add("@Carbohydrates", SqlDbType.Float).Value = product.Carbohydrates;
            cmd.Parameters.Add("@GrammInUnit", SqlDbType.Int).Value = product.GrammInUnit;
            cmd.Parameters.Add("@BreadUnits", SqlDbType.Float).Value = product.BreadUnits;

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
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
        }

        static public bool updateProductGroups(ProductGroups productGroups)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            string query = "UPDATE [dbo].[Groups] " +
                "SET [Name] = @Name," +
                "[Image] = @Image " +
                "WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = productGroups.GroupID;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = productGroups.GroupName;
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = productGroups.GroupImage;

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
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
        }

        static public bool updateAreaTable(PersonalArea product)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            string query = "UPDATE [dbo].[PersonalArea] " +
                "SET " +
                "[UserID] = @UserID," +
                "[Product] = @Product," +
                "[ProductGroup] = @ProductGroup," +
                "[Carbohydrates] = @Carbohydrates," +
                "[GrammInUnit] = @GrammInUnit," +
                "[BreadUnits] = @BreadUnits " +
                "[DateCreate] = @DateCreate" +
                "WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = product.ID;
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = product.UserID;
            cmd.Parameters.Add("@Product", SqlDbType.NVarChar).Value = product.Product;
            cmd.Parameters.Add("@ProductGroup", SqlDbType.NVarChar).Value = product.ProductGroupName;
            cmd.Parameters.Add("@Carbohydrates", SqlDbType.Float).Value = product.Carbohydrates;
            cmd.Parameters.Add("@GrammInUnit", SqlDbType.Int).Value = product.GrammInUnit;
            cmd.Parameters.Add("@BreadUnits", SqlDbType.Float).Value = product.BreadUnits;
            cmd.Parameters.Add("@DateCreate", SqlDbType.DateTime).Value = product.DateCreate;

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
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
        }
    }
}
