using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;

namespace WpfUniverse.Common.Requests
{
   public class UncheckAllPropertiesRequest:Request<DoneResponse>
    {
        public int PlanetId { get; }

        public UncheckAllPropertiesRequest(int planetId)
        {
            PlanetId = planetId;
        }
    }
}
