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
    public class Delete
    {
        public static string exception { get; set; }

        static public bool deleteBreadUnitsTable(Products product)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            string query = "DELETE FROM [dbo].[BreadUnitsTable] WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = product.ID;

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

        static public bool deleteProductGroups(ProductGroups productGroups)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            string query = "DELETE FROM [dbo].[Groups] WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = productGroups.GroupID;

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


        static public bool deleteAreaTable(PersonalArea product)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            string query = "DELETE FROM [dbo].[PersonalArea] WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = product.ID;

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
