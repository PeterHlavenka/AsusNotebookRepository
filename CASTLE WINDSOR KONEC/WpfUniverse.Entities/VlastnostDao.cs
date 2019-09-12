using System.Data;
using System.Linq;
using BLToolkit.Data.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace WpfUniverse.Entities
{
    public class VlastnostDao : EntityDaoBase<Vlastnost, UniverseDataModel, int, Vlastnost>, IVlastnostDao
    {
        public VlastnostDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public VlastnostDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }


        public override void Delete(Vlastnost vlastnost)
        {
            using (var model = CreateDbContext())
            {
                model.VlastnostiPlanet.Where(x => x.VlastnostId == vlastnost.Id).Delete();

                Delete(vlastnost, model);
            }
        }

        public override void Insert(Vlastnost vlastnost)
        {
            using (var model = CreateDbContext())
            {
                var id = model.Vlastnost.Max(x => x.Id) + 1;

                vlastnost.Id = id;

                Insert(vlastnost, model);
            }
        }
    }


    public interface IVlastnostDao : ISimpleDao<Vlastnost, int>
    {
    }
}