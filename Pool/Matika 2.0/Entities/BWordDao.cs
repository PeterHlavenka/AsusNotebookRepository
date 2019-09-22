using System.Data;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace Entities
{
   public class BWordDao: EntityIdentityKeyDaoBase<BWord, EnumeratedWordsDataModel, int, BWord>, IWordDao
    {
        public BWordDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public BWordDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }

    }

    public interface IWordDao : ISimpleDao<BWord, int>
    {
    }
}
