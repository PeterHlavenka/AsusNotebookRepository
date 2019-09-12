using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Responses;

namespace WpfUniverse.Common.Requests
{
   public class DeleteVlastnostsFromPlanetRequest :Request<DeleteVlastnostsFromPlanetResponse>
    {
        public int Id { get; }

        public DeleteVlastnostsFromPlanetRequest(int id)
        {
            Id = id;
        }
    }
}
