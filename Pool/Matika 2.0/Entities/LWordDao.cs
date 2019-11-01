using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace Entities
{
    public class LWordDao : EntityIdentityKeyDaoBase<LWord, EnumeratedWordsDataModel, int, LWord>, ILWordDao
    {
        public LWordDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public LWordDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }

        public IEnumerable<LWord> GetWords()
        {
            using (var model = CreateDbContext())
            {
                return model.LWord.Select(d => d);
            }
        }
    }

    public interface ILWordDao : ISimpleDao<LWord, int>
    {
        IEnumerable<LWord> GetWords();
    }
}
