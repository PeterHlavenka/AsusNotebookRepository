using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Common.Responses
{
   public class SelectPlanetsResponse :Response
    {
        public SelectPlanetsResponse(List<PlanetDataContract> planets)
        {
            Planets = planets;
        }

        public List<PlanetDataContract> Planets { get; private set; }
    }
}
