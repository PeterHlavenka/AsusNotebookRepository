using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Common.Responses
{
   public class GetCheckedResponse:Response
    {
        public List<VlastnostDataContract> Vlastnosts { get; }
        public GetCheckedResponse(List<VlastnostDataContract> vlastnosts)
        {
            Vlastnosts = vlastnosts;
        }
    }
}
