using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Responses;

namespace WpfUniverse.Common.Requests
{
   public class InsertPlanetRequest:Request<InsertPlanetResponse>
    {
        public PlanetDataContract PlanetDataContract { get; }

        public InsertPlanetRequest(PlanetDataContract planetDataContract)
        {
            PlanetDataContract = planetDataContract;
        }
    }
}
