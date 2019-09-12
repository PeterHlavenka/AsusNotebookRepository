using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Common.Responses
{
  public  class UpdatePlanetResponse:Response
    {
        public PlanetDataContract Planet { get; }

        public UpdatePlanetResponse(PlanetDataContract planet)
        {
            Planet = planet;
        }
    }
}
