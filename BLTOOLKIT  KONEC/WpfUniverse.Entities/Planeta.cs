using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace WpfUniverse.Entities
{
    [DaoFactory(DaoType = typeof(PlanetDao))]
    [TableName("Planeta", Owner = "dbo")]
    public class Planeta: LightDatabaseEntityIdentityIntKey<Planeta>
    {




        public string Jmeno { get; set; }

        public int Velikost { get; set; }

        public int GalaxieId { get; set; }

        public Guid Identifikator { get; set; }

        public List<Vlastnost> Properties { get; set; }       //planeta uz tuhle vlastnost ma rovnou pri nacteni z db pomoci join query.
    }
}
