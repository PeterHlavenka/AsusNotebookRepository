using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;

namespace WpfUniverse.Common.Requests
{
   public class UpdateNameOfPropertyRequest : Request<DoneResponse>
    {
        public string Name { get; }
        public int PropertyId { get; }

        public UpdateNameOfPropertyRequest(string name, int propertyId)
        {
            Name = name;
            PropertyId = propertyId;
        }
    }
}
