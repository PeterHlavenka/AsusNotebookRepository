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
    public class VlastnostDao :  EntityDaoBase<Vlastnost, UniverseDataModel,int, Vlastnost>, IVlastnostDao
    {
       

        public VlastnostDao(string dbAlias)
            : base(dbAlias)
        {

        }

        public VlastnostDao(string dbAlias, IsolationLevel isolationLevel )
            :base(dbAlias, isolationLevel)
        {

        }

        public override void Insert(Vlastnost obj)
        {
            using (UniverseDataModel model = CreateDbContext())
            {
                try
                {
                    model.BeginTransaction(IsolationLevel.Serializable);

                    int id = model.Vlastnost.Max(x => x.Id) + 1;

                    obj.Id = id;

                    Insert(obj, model);

                    model.CommitTransaction();
                }
                catch(Exception)
                {
                    model.RollbackTransaction();
                    throw;
                }
            }
        }

        public void DeleteVlastnost(Vlastnost vlastnost)
        {
            using (UniverseDataModel model = CreateDbContext())
            {

                model.VlastnostiPlanet.Where(x => x.VlastnostId == vlastnost.Id).Delete();      //vymaze vazaci tabulky kde figuruje VlastnostId  == vlastnost.Id

                model.Vlastnost.Where(x => x.Id == vlastnost.Id).Delete();       // vymaze vlastnost
            }
        }


       
    }





    public interface IVlastnostDao : ISimpleDao<Vlastnost, int>
    {
    
        void DeleteVlastnost(Vlastnost vlastnost);

        //void InsertVlastnost(Vlastnost vlastnost);

    }

}
