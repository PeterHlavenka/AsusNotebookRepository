using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
    public class VlastnostiPlanetDao : EntityDaoBase<VlastnostiPlanet>
    {
        public VlastnostiPlanetDao(string connectionString) : base(connectionString)
        {

        }

        public List<VlastnostiPlanet> LoadAllVlastnostiPlanet()
        {
            string query = "Select PlanetaId, VlastnostId from dbo.VlastnostiPlanet";

            return LoadEntity(query, (SqlDataReader reader) => new VlastnostiPlanet()
            {
                PlanetaId = reader.GetInt32(0),
                VlastnostId = reader.GetInt32(1)
            });
        }

        public void InserPropertyToPlanet(int planetId, int propertyId)
        {
            string query = "INSERT INTO dbo.VlastnostiPlanet (PlanetaId, VlastnostId) VALUES (@PlanetaId, @VlastnostId) ";

            SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@PlanetaId", planetId),
                    new SqlParameter("@VlastnostId", propertyId)
                };

            ExcuteUpdate(query, parameters);
        }

        public void RemovePropertyFromPlanet(int planetId, int propertyId)
        {
            string query = "DELETE FROM dbo.VlastnostiPlanet WHERE PlanetaID = @PlanetaId AND VlastnostId = @VlastnostId";

            SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@PlanetaId", planetId),
                    new SqlParameter("@VlastnostId", propertyId)
                };

            ExcuteUpdate(query, parameters);
        }

        public void RemovAllVlastnostiPlanet(int planetId)
        {
            string query = "DELETE FROM dbo.VlastnostiPlanet WHERE PlanetaID = @PlanetaId ";

            SqlParameter[] parameters = new[]
                {
                 new SqlParameter("@PlanetaId", planetId)
                };
            ExcuteUpdate(query, parameters);
        }
    }
}
