using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Mediaresearch.Framework.Utilities.Net
{
	/// <summary>
	/// Je tu proto, aby se dalo lepe testovat (jinak by se samozrejme dalo pouzit primo <see cref="NetworkInterface"/>
	/// </summary>
	public interface INetworkInterface
	{
		List<IpAddressSubnetMaskPair> GetAllNetworkInterfaces();
	}
}