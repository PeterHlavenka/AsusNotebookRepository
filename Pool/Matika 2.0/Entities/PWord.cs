using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace Entities
{
    [DaoFactory(DaoType = typeof(PWordDao))]
    [TableName("P_Word", Owner = "dbo")]
    public class PWord : LightDatabaseEntityIdentityIntKey<PWord>, IWord
    {
        public string Name { get; set; }
        public bool IsEnumerated { get; set; }
        public string CoveredName { get; set; }
        public string Help { get; set; }
    }
}