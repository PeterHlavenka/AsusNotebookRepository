using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
   public class VlastnostDao:EntityDaoBase<Vlastnost>
    {
        //konstruktor ziska connectionString z konstruktoru rodicovske tridy
        public VlastnostDao(string connectionString ): base(connectionString)
        {

        }

        //metoda list planet
        public List<Vlastnost> LoadAllVlastnosts ()
        {
            //sql dotaz
            string query = "Select Id, Nazev from dbo.Vlastnost";

            //vracime vysledek metody LoadEntity v rodicovske tride . Jako parametr metode predame 
            //query a instanci Sqlreaderu ktery ma nactenou instanci entity / vlastnosti

            return LoadEntity(query, (SqlDataReader reader) => new Vlastnost()
            {
                Id = reader.GetInt32(0),
                Nazev = reader.GetString(1)
            });
        }

       
    }
}
