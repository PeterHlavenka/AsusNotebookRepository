using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
    public class GalaxyDao : EntityDaoBase<Galaxie>
    {
        public GalaxyDao(string connectionString)  : base(connectionString)          
        {
        }

        public List<Galaxie> LoadAllGalaxies()
        {
            string query = "Select Id, Jmeno, PolohaX, PolohaY, PolohaZ from dbo.Galaxie";
            

            return LoadEntity(query, (SqlDataReader reader) => new Galaxie()
            {
                Id = reader.GetInt32(0),
                Jmeno = reader.GetString(1),
                PolohaX = reader.GetInt64(2),
                PolohaY = reader.GetInt64(3),
                PolohaZ = reader.GetInt64(4)
            });
        }


        public void SaveGalaxy(Galaxie galaxie)
        {

            //dulezite je tam to WHERE jinak to prepise celou tabulku;
            string query = "UPDATE dbo.Galaxie SET Jmeno = @Jmeno, PolohaX = @PolohaX, PolohaY = @PolohaY,PolohaZ =@PolohaZ WHERE Id = @Id";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@Id" , galaxie.Id),
                new SqlParameter("@Jmeno", galaxie.Jmeno),
                new SqlParameter("@PolohaX", galaxie.PolohaX),
                new SqlParameter("@PolohaY" , galaxie.PolohaY),
                new SqlParameter("@PolohaZ" , galaxie.PolohaZ)
            };

            ExcuteUpdate(query, parameters);
        }
    }
}
