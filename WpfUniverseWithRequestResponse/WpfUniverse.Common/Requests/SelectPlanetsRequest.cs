using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;

using WpfUniverse.Common.Responses;

namespace WpfUniverse.Common.Requests
{
   public class SelectPlanetsRequest:Request<SelectPlanetsResponse>
    {
        public SelectPlanetsRequest(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
