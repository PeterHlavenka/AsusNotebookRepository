using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;

namespace WpfUniverse.Common.Requests
{
   public class RemoveSelectedPropertyRequest : Request<DoneResponse>

    {
        public int PlanetId { get; }
        public int VlastnostId { get; }

        public RemoveSelectedPropertyRequest(int planetId, int vlastnostId)
        {
            PlanetId = planetId;
            VlastnostId = vlastnostId;
        }

    }
}
