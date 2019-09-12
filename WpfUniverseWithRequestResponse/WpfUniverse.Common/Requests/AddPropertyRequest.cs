using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Responses;

namespace WpfUniverse.Common.Requests
{
  public  class AddPropertyRequest:Request<AddPropertyResponse>
    {
        public string PropertyName { get; }

        public AddPropertyRequest(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
