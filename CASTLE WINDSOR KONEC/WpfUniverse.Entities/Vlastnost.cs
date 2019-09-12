using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;


namespace WpfUniverse.Entities
{
    [DaoFactory(DaoType = typeof(VlastnostDao))]
    [TableName("Vlastnost", Owner = "dbo")]
   public class Vlastnost : LightDatabaseEntityIntKey<Vlastnost>
    {  
        public string Nazev { get; set; }           
    }
}
