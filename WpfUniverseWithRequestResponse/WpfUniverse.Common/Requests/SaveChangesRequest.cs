using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Common.Requests
{
   public class SaveChangesRequest:Request<DoneResponse>
    {
        public List<VlastnostDataContract> List { get; }
        public int SelectedPlanetId { get; }

        public SaveChangesRequest(List<VlastnostDataContract> list,  int selectedPlanetId)
        {
            List = list;
            SelectedPlanetId = selectedPlanetId;
        }
    }
}
