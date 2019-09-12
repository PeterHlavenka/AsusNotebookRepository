using BLToolkit.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;
using BLToolkit.Data;
using Mediaresearch.Framework.DataAccess.BLToolkit.Interceptors;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using System.Data;

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
