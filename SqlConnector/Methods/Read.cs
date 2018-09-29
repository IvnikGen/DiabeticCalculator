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
    public class Read
    {
        public static string exception { get; set; }

        static public List<Products> getBreadUnitsTable()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            List<Products> liProd = new List<Products>();

            try
            {
                List<ProductGroups> lpg = getProductGroups();
                connection.Open();

                using (var command = new SqlCommand(@"SELECT * FROM [dbo].[BreadUnitsTable]", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 0;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        liProd.Add(new Products
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Product = reader["Product"].ToString(),
                            ProductGroup = Convert.ToInt32(reader["ProductGroup"]),
                            Carbohydrates = Math.Round(Convert.ToSingle(reader["Carbohydrates"]),2),
                            GrammInUnit = reader["GrammInUnit"] == DBNull.Value ? 0 : Convert.ToInt32(reader["GrammInUnit"]),
                            BreadUnits = reader["BreadUnits"] == DBNull.Value ? 0 : Math.Round(Convert.ToSingle(reader["BreadUnits"]),2),
                            ProductGroupName = lpg.FirstOrDefault(x=> x.GroupID == Convert.ToInt32(reader["ProductGroup"])).GroupName
                        });
                    }
                }
            }
            catch (Exception e)
            {
                exception = e.Message;
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

            return liProd;
        }

        static public List<ProductGroups> getProductGroups()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            List<ProductGroups> liProdGroups = new List<ProductGroups>();

            try
            {
                connection.Open();

                using (var command = new SqlCommand(@"SELECT * FROM [dbo].[Groups]", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 0;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        liProdGroups.Add(new ProductGroups
                        {
                            GroupID = Convert.ToInt32(reader["ID"]),
                            GroupName = reader["Name"].ToString(),
                            GroupImage = reader["Image"] == DBNull.Value ? "" : reader["Image"].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                exception = e.Message;
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

            return liProdGroups;
        }

        static public List<PersonalArea> getAreaTable()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dboBreadUnits"].ConnectionString);
            List<PersonalArea> liProd = new List<PersonalArea>();

            try
            {
                connection.Open();

                using (var command = new SqlCommand(@"SELECT * FROM [dbo].[PersonalArea]", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 0;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["Product"] != DBNull.Value)
                        {
                            DateTime dt = DateTime.Parse(reader["DateCreate"].ToString());

                            liProd.Add(new PersonalArea
                            {
                                ID = Convert.ToInt32(reader["ID"].ToString()),
                                UserID = reader["UserID"].ToString().Trim(),
                                Product = reader["Product"].ToString().Trim(),
                                Carbohydrates = Math.Round(Convert.ToSingle(reader["Carbohydrates"]), 2),
                                GrammInUnit = reader["GrammInUnit"] == DBNull.Value ? 0 : Convert.ToInt32(reader["GrammInUnit"].ToString()),
                                BreadUnits = reader["BreadUnits"] == DBNull.Value ? 0 : Math.Round(Convert.ToSingle(reader["BreadUnits"].ToString()), 2),
                                ProductGroupName = reader["ProductGroup"].ToString().Trim(),
                                DateCreate = reader["DateCreate"] == DBNull.Value ? DateTime.Now: dt
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                exception = e.Message;
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

            return liProd;
        }
    }
}
