using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace WpfUniverse.Entities
{

    [DaoFactory(DaoType = typeof(GalaxyDao))]
    [TableName("Galaxie", Owner = "dbo")]
    public class Galaxie : LightDatabaseEntityIdentityIntKey<Galaxie>
    {
        public string Jmeno { get; set; }

        public long PolohaX { get; set; }

        public long PolohaY { get; set; }

        public long PolohaZ { get; set; }    
    }
}