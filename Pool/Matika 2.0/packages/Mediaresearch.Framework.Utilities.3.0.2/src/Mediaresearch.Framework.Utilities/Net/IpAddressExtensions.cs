using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace Mediaresearch.Framework.Utilities.Net
{
	/// <summary>
	/// Prevzato z http://blogs.msdn.com/b/knom/archive/2008/12/31/ip-address-calculations-with-c-subnetmasks-networks.aspx
	///  + pridany vlastni metody
	/// </summary>
	public static class IpAddressExtensions
	{
		private static readonly INetworkInterface m_defaultNetworkInterface = new DefaultNetworkInterface();

		public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
		{
			byte[] ipAdressBytes = address.GetAddressBytes();
			byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

			if (ipAdressBytes.Length != subnetMaskBytes.Length)
				throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

			byte[] broadcastAddress = new byte[ipAdressBytes.Length];
			for (int i = 0; i < broadcastAddress.Length; i++)
			{
				broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
			}
			return new IPAddress(broadcastAddress);
		}

		public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
		{
			byte[] ipAdressBytes = address.GetAddressBytes();
			byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

			if (ipAdressBytes.Length != subnetMaskBytes.Length)
				throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

			byte[] broadcastAddress = new byte[ipAdressBytes.Length];
			for (int i = 0; i < broadcastAddress.Length; i++)
			{
				broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
			}
			return new IPAddress(broadcastAddress);
		}

		public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
		{
			if (subnetMask == null)
				return false;

			IPAddress network1 = address.GetNetworkAddress(subnetMask);
			IPAddress network2 = address2.GetNetworkAddress(subnetMask);

			return network1.Equals(network2);
		}

		public static bool IsInSameSubnetAsAnInterNetworkIpAddress(this IPAddress address)
		{
			List<IpAddressSubnetMaskPair> interNetworkIpAddresses = GetInterNetworkIpAddresses();
			return interNetworkIpAddresses.Any(ip => ip.IpAddress != null && ip.SubnetMask != null && address.IsInSameSubnet(ip.IpAddress, ip.SubnetMask));
		}

		public static List<IpAddressSubnetMaskPair> GetInterNetworkIpAddresses()
		{
			return GetInterNetworkIpAddresses(m_defaultNetworkInterface);
		}

		/// <summary>
		/// Je tu kvuli testum - normalne by se melo pouzivate jen <see cref="GetInterNetworkIpAddresses()"/>
		/// </summary>
		public static List<IpAddressSubnetMaskPair> GetInterNetworkIpAddresses(INetworkInterface networkInterface)
		{
			return networkInterface.GetAllNetworkInterfaces();
		}
	}
}