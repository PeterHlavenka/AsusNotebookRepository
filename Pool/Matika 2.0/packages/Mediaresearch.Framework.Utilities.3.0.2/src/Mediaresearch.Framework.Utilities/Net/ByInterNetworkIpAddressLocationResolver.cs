using System.Collections.Generic;
using System.Linq;

namespace Mediaresearch.Framework.Utilities.Net
{
	public class ByInterNetworkIpAddressLocationResolver<T> where T : class, ILocation
	{
		private readonly List<T> m_locations = new List<T>(); 
		public ByInterNetworkIpAddressLocationResolver(IEnumerable<T> locations)
		{
			if (locations == null)
				return;
			Locations.AddRange(locations.Where(l => l != null));
		}

		public List<T> Locations
		{
			get { return m_locations; }
		}

		public T GetLocation()
		{
			return Locations.FirstOrDefault(l => l.IpAddressInSubnet != null && l.IpAddressInSubnet.IsInSameSubnetAsAnInterNetworkIpAddress());
		}
	}
}