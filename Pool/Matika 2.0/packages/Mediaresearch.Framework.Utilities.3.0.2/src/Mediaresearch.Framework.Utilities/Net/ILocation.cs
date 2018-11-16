using System.Net;

namespace Mediaresearch.Framework.Utilities.Net
{
	public interface ILocation
	{
		IPAddress IpAddressInSubnet { get; }
	}
}