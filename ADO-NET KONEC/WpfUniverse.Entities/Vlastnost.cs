using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
   public class Vlastnost
    {

        public int Id { get; set; }

        public string Nazev { get; set; }

        public static List<Vlastnost> LoadAllVlastnosts(string connectionString)
        {
            string query = "Select Id, Nazev from dbo.Galaxie";

            List<Vlastnost> result = new List<Vlastnost>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Vlastnost vl = new Vlastnost()
                                {
                                    Id = reader.GetInt32(0),
                                    Nazev = reader.GetString(1),
                                   
                                };

                                result.Add(vl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }

            return result;
        }
    }
}
