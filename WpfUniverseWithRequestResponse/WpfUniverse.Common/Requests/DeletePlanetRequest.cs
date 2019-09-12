using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Responses;

namespace WpfUniverse.Common.Requests
{
   public class DeletePlanetRequest: Request<DoneResponse>
    {
        public DeletePlanetRequest(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
