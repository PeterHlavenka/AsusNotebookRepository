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
  public  class UpdatePlanetRequest:Request<UpdatePlanetResponse>
    {
        public PlanetDataContract PlanetDataContract { get; }


        public UpdatePlanetRequest(PlanetDataContract planetDataContract)
        {
            PlanetDataContract = planetDataContract;
        }
    }
}
