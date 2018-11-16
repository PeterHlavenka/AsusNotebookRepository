using System.Net;

namespace Mediaresearch.Framework.Utilities.Net
{
	public class IpAddressSubnetMaskPair
	{
		public IpAddressSubnetMaskPair(IPAddress ipAddress, IPAddress subnetMask)
		{
			IpAddress = ipAddress;
			SubnetMask = subnetMask;
		}

		public IPAddress IpAddress { get; private set; }
		public IPAddress SubnetMask { get; private set; }
	}
}