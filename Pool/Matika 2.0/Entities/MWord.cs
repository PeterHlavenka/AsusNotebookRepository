using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace Entities
{
    [DaoFactory(DaoType = typeof(MWordDao))]
    [TableName("M_Word", Owner = "dbo")]
    public class MWord : LightDatabaseEntityIdentityIntKey<MWord>, IWord
    {
        public string Name { get; set; }
        public bool IsEnumerated { get; set; }
        public string CoveredName { get; set; }
        public string Help { get; set; }
    }
}