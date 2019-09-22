using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace Entities
{
   public class BWordDao: EntityIdentityKeyDaoBase<BWord, EnumeratedWordsDataModel, int, BWord>, IBWordDao
    {
        public BWordDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public BWordDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }

        public IEnumerable<BWord> GetWords()
        {
            using (var model = CreateDbContext())
            {                
                return model.BWord.Select(d => d);           
            }
        }
    }

    public interface IBWordDao : ISimpleDao<BWord, int>
    {
        IEnumerable<BWord> GetWords();
    }
}
