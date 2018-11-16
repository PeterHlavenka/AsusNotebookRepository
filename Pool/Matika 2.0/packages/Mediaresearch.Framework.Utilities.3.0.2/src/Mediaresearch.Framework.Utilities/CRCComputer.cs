using System.Collections.Generic;

namespace Mediaresearch.Framework.Utilities
{
	/// <summary>
	/// Poskytuje metody pro vypocet vsech standartnich typu CRC
	/// </summary>
	public static class CRCComputer
	{
		/// <summary>
		/// Vraci CRC 16 z predanych dat. Initial value = 0xFFFF, polynom x^16 + x^15 + x^2 + 1 (0xA001)
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <returns>CRC 16</returns>
		public static ushort ComputeCRC16(IEnumerable<byte> data)
		{
			return ComputeCRC16(data, 0xFFFF, 0xA001);
		}

		/// <summary>
		/// Vraci CRC 16 z predanych dat.
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <param name="initialValue">pocatecni hodnota CRC</param>
		/// <param name="polynom">polynom</param>
		/// <returns>CRC 16</returns>
		public static ushort ComputeCRC16(IEnumerable<byte> data, ushort initialValue, ushort polynom)
		{
			ushort crc = initialValue;
			foreach (byte b in data)
			{
				crc ^= b;
				for (int i = 0; i < 8; ++i)
					crc = (ushort) ((crc & 1) > 0 ? (crc >> 1) ^ polynom : crc >> 1);
			}

			return crc;
		}

		/// <summary>
		/// Vraci CRC CCITT z predanych dat. Initial value = 0xFFFF, polynom = x^16 + x^12 + x^5 + 1 (0x8408)
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <returns>CRC 16</returns>
		public static ushort ComputeCRC_CCITT(IEnumerable<byte> data)
		{
			return ComputeCRC_CCITT(data, 0xFFFF);
		}

		/// <summary>
		/// Vraci CRC CCITT z predanych dat, polynom = x^16 + x^12 + x^5 + 1 (0x8408)
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <param name="initialValue">pocatecni hodnota CRC</param>
		/// <returns>CRC CCITT</returns>
		public static ushort ComputeCRC_CCITT(IEnumerable<byte> data, ushort initialValue)
		{
			ushort crc = initialValue;
			foreach (byte b in data)
			{
				byte d = b;

				d ^= (byte)(crc & 0xFF);
				d ^= (byte)(d << 4);

				crc = (ushort)(((d << 8) | (byte)(crc >> 8)) ^ (byte)(d >> 4) ^ (d << 3));
			}

			return crc;
		}

		/// <summary>
		/// Vraci CRC 8 z predanych dat. Initial value = 0x00, polynom x^8 + x^5 + x^4 + 1 (0x8C)
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <returns>CRC 8</returns>
		public static byte ComputeCRC8(IEnumerable<byte> data)
		{
			return ComputeCRC8(data, 0x00, 0x8C);
		}

		/// <summary>
		/// Vraci CRC 8 z predanych dat.
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <param name="initialValue">pocatecni hodnota CRC</param>
		/// <param name="polynom">polynom</param>
		/// <returns>CRC 8</returns>
		public static byte ComputeCRC8(IEnumerable<byte> data, byte initialValue, byte polynom)
		{
			byte crc = initialValue;
			foreach (byte b in data)
			{
				crc = (byte) (crc ^ b);
				for (byte i = 0; i < 8; i++)
					crc = (byte)((crc & 0x01) > 0 ? ((crc >> 1) ^ polynom) : crc >> 1);
			}

			return crc;
		}

		/// <summary>
		/// Vraci CRC XMODEM z predanych dat. Initial value = 0x00, polynom x^16 + x^12 + x^5 + 1 (0x1021)
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <returns>CRC XMODEM</returns>
		public static ushort ComputeCRC_XMODEM(IEnumerable<byte> data)
		{
			return ComputeCRC_XMODEM(data, 0x00, 0x1021);
		}

		/// <summary>
		/// Vraci CRC XMODEM z predanych dat.
		/// </summary>
		/// <param name="data">data pro vypocet CRC</param>
		/// <param name="initialValue">pocatecni hodnota CRC</param>
		/// <param name="polynom">polynom</param>
		/// <returns>CRC XMODEM</returns>
		public static ushort ComputeCRC_XMODEM(IEnumerable<byte> data, ushort initialValue, ushort polynom)
		{
			ushort crc = initialValue;
			
			foreach (byte b in data)
			{
				crc = (ushort) (crc ^ (b << 8));

				for (byte i = 0; i < 8; i++)
					crc = (ushort) ((crc & 0x8000) > 0 ? ((crc << 1) ^ polynom) : crc << 1);
			}

			return crc;
		}
	}
}
