using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace Entities
{
    [DaoFactory(DaoType = typeof(LWordDao))]
    [TableName("L_Word", Owner = "dbo")]
    public class LWord : LightDatabaseEntityIdentityIntKey<LWord>, IWord
    {
        public string Name { get; set; }
        public bool IsEnumerated { get; set; }
        public string CoveredName { get; set; }
        public string Help { get; set; }
    }
}