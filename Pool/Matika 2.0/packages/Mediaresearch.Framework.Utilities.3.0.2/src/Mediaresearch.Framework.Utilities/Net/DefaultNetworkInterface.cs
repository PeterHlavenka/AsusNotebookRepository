using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Mediaresearch.Framework.Utilities.Net
{
	public class DefaultNetworkInterface : INetworkInterface
	{
		public List<IpAddressSubnetMaskPair> GetAllNetworkInterfaces()
		{
			List<IpAddressSubnetMaskPair> result = new List<IpAddressSubnetMaskPair>();
			foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
			{
				result.AddRange(adapter.GetIPProperties().UnicastAddresses
									.Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
									.Select(ip => new IpAddressSubnetMaskPair(ip.Address, ip.IPv4Mask)));
			}
			return result;
		}
	}
}