using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
    public abstract class EntityDaoBase<TEntity>
    {
        protected EntityDaoBase(string connectionString)
        {
            ConnectionString = connectionString;   //do protected promenne priradime connectionString 
        }

        protected string ConnectionString { get; }

        /// <summary>
        /// vraci list of entity, jmenuje se LoadEntity, jako parametr si bere "nejakou entitu" a  jako parametr dostane dotaz a SqlDataReader ,ktery uz ma nactenou entitu i s vlastnostmi
        /// parametry jsou null tj nepovinne
        /// </summary>
        /// <typeparam name="TEntity">jakakoli entita napr galaxie nebo planeta</typeparam>
        /// <param name="query">je sql dotaz ve kterem urcujeme jake vlastnosti budeme z databaze potrebovat je definovany ve zdedenych tridach</param>
        /// <param name="factoryMethod">metoda definovana ve zdedenych tridach vytvori objekt podle tridy ve ktere je volane a priradi mu vlastnosti vybrane v query</param>
        /// <returns>vraci seznam entit podle toho ve ktere zdedene tride je metoda volana</returns>
        protected List<TEntity> LoadEntity(string query, Func<SqlDataReader, TEntity> factoryMethod, SqlParameter[] parameters = null)
        {
            List<TEntity> result = new List<TEntity>();   

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    conn.Open();

                    transaction = conn.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(query, conn, transaction))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var vlp = factoryMethod.Invoke(reader);

                                result.Add(vlp);
                            }
                        }
                    }

                    transaction.Commit();    //  Neco jako Submit
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();    //   Vrat se zpatky a neprovadej zadne zmeny
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


        protected int ExcuteInsert(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    conn.Open();

                    transaction = conn.BeginTransaction();

                    int id;

                    using (SqlCommand command = new SqlCommand(query, conn, transaction))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        id = Convert.ToInt32(command.ExecuteScalar());
                    }

                    transaction.Commit();

                    return id;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();

                    Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        protected void ExcuteUpdate(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    conn.Open();

                    transaction = conn.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(query, conn, transaction))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();

                    Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }


      
    }
}
