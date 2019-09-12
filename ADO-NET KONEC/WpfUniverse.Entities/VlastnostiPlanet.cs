using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
    public class VlastnostiPlanet
    {

        public int PlanetaId { get; set; }

        public int VlastnostId { get; set; }


        public static List<TEntity> LoadEntity<TEntity>(string connectionString, string query, Func<SqlDataReader, TEntity> factoryMethod)
        {
            List<TEntity> result = new List<TEntity>();

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
                                var vlp = factoryMethod.Invoke(reader);

                                result.Add(vlp);
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


        public static List<VlastnostiPlanet> LoadAllVlastnostiPlanets(string connectionString)
        {
            string query = "Select PlanetaId, VlastnostId from dbo.VlastnostiPlanet";

            Func<SqlDataReader, VlastnostiPlanet> func = (SqlDataReader reader) => new VlastnostiPlanet()
            {
                PlanetaId = reader.GetInt32(0),
                VlastnostId = reader.GetInt32(1)
            };

            return LoadEntity(connectionString, query, func);
        }


    }
}
