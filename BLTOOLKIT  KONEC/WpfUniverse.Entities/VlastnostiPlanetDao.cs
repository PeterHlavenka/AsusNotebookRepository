using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using System.Data;


namespace WpfUniverse.Entities
{
    public class VlastnostiPlanetDao : EntityMultiKeyDaoBase<VlastnostiPlanet, UniverseDataModel, VlastnostiPlanet>, IVlastnostiPlanetDao
    {

        public VlastnostiPlanetDao(string dbAlias)
             : base(dbAlias)
        {

        }

        public VlastnostiPlanetDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {

        }


        public void RemoveAllVlastnostiPlanet(int selectedPlanet_Id)
        {
            using (UniverseDataModel model = CreateDbContext())
            {

                model.VlastnostiPlanet.Where(x => x.PlanetaId == selectedPlanet_Id).Delete();

            }
        }


        public void InsertPropertyToPlanet(VlastnostiPlanet vlastnostiPlanet)
        {
            using (UniverseDataModel model = CreateDbContext())
            {
                //stejne vola metodu ktera ma dva parametry, jen ji preda novou instanci modelu
                InsertPropertyToPlanet(vlastnostiPlanet, model);
            }
        }

        public void InsertPropertyToPlanet(VlastnostiPlanet vlastnostiPlanet, UniverseDataModel model)
        {
            // query dostane navic do parametru navic model ktery jsme dostali z volajici metody
            SqlQuery<VlastnostiPlanet> query = new SqlQuery<VlastnostiPlanet>(model);
            query.Insert(vlastnostiPlanet);
        }


        public void RemovePropertyFromPlanet(int selectedPlanetId, int vlastnostId)
        {
            using (UniverseDataModel model = CreateDbContext())
            {
                RemovePropertyFromPlanet(selectedPlanetId, vlastnostId, model);
            }

        }

        public UniverseDataModel CreateDbModel()
        {
            return CreateDbContext();
        }

        public void RemovePropertyFromPlanet(int selectedPlanetId, int vlastnostId, UniverseDataModel model)
        {
            model.VlastnostiPlanet.Where(x => x.PlanetaId == selectedPlanetId && x.VlastnostId == vlastnostId).Delete();
        }
    }


    public interface IVlastnostiPlanetDao : IEntityMultiKeyDaoBase<VlastnostiPlanet>
    {
        void RemoveAllVlastnostiPlanet(int id);
        void InsertPropertyToPlanet(VlastnostiPlanet vlastnostiPlanet, UniverseDataModel model);
        void RemovePropertyFromPlanet(int id1, int id2, UniverseDataModel model);
        void InsertPropertyToPlanet(VlastnostiPlanet vp);
        void RemovePropertyFromPlanet(int id1, int id2);

        UniverseDataModel CreateDbModel();
    }
}
