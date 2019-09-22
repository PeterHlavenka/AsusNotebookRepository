using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace Entities
{
    [DaoFactory(DaoType = typeof(BWordDao))]
    [TableName("BWord", Owner = "dbo")]
   public class BWord : LightDatabaseEntityIdentityIntKey<BWord>
    {
        public string Name { get; set; }
        public bool IsEnumerated { get; set; }
        public string CoveredName { get; set; }
        public string Help { get; set; }
    }
}
