using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace WpfUniverse.Entities
{

    [DaoFactory(DaoType = (typeof(VlastnostiPlanetDao)))]
    [TableName("VlastnostiPlanet", Owner = "dbo")]
    public class VlastnostiPlanet : LightDatabaseEntityMultiKeyBase<VlastnostiPlanet>
    {
        [PrimaryKey(0)]
        public int PlanetaId { get; set; }
        [PrimaryKey(1)]
        public int VlastnostId { get; set; }      
    }
}
