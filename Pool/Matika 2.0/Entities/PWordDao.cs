using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;

namespace Entities
{
    public class PWordDao : EntityIdentityKeyDaoBase<PWord, EnumeratedWordsDataModel, int, PWord>, IPWordDao
    {
        public PWordDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public PWordDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }

        public IEnumerable<PWord> GetWords()
        {
            using (var model = CreateDbContext())
            {
                return model.PWord.Select(d => d);
            }
        }
    }

    public interface IPWordDao : ISimpleDao<PWord, int>
    {
        IEnumerable<PWord> GetWords();
    }
}