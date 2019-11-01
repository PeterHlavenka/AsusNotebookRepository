using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace Entities
{
    [DaoFactory(DaoType = typeof(BWordDao))]
    [TableName("B_Word", Owner = "dbo")]
   public class BWord : LightDatabaseEntityIdentityIntKey<BWord>, IWord
    {
        public string Name { get; set; }
        public bool IsEnumerated { get; set; }
        public string CoveredName { get; set; }
        public string Help { get; set; }
    }
}
