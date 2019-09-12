using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Common.Responses
{
  public  class InsertPlanetResponse : Response
    {
        public PlanetDataContract PlanetDataContract { get; }

        public InsertPlanetResponse(PlanetDataContract planetDataContract)
        {
            PlanetDataContract = planetDataContract;
        }
    }
}
