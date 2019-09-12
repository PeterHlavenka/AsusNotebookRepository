using System.Data;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace WpfUniverse.Entities
{
    public class GalaxyDao : EntityIdentityKeyDaoBase<Galaxie, UniverseDataModel, int, Galaxie>, IGalaxyDao
    {
        public GalaxyDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public GalaxyDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }
    }

    public interface IGalaxyDao : ISimpleDao<Galaxie, int>
    {
    }
}