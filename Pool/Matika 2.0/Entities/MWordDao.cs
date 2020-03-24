using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace Entities
{
    public class MWordDao : EntityIdentityKeyDaoBase<MWord, EnumeratedWordsDataModel, int, MWord>, IMWordDao
    {
        public MWordDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public MWordDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }

        public IEnumerable<MWord> GetWords()
        {
            using (var model = CreateDbContext())
            {
                return model.MWord.Select(d => d);
            }
        }
    }

    public interface IMWordDao : ISimpleDao<MWord, int>
    {
        IEnumerable<MWord> GetWords();
    }
}