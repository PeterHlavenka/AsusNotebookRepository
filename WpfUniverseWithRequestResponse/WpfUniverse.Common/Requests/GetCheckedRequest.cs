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
    
   public class GetCheckedRequest: Request<GetCheckedResponse>
    {
        public int Id { get; }

        public GetCheckedRequest(int id)
        {
            Id = id;
        }
    }
}
