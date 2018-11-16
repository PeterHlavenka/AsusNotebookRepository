using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mediaresearch.Framework.Utilities
{
	/// <summary> 
	/// Summary description for NumericalSystemConverter.
	/// </summary>
	public class NumericalSystemConverter
	{
		// [] Digits : statické pole, které využivají statické funkce pro pøevody èíslených soustav.
		private static readonly char[] m_digits =
			new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'X'};

		private static readonly byte[] m_bitMaskArray = new byte[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
		
		public const string HexPrefix = "0x";

		public static byte[] BitMaskArray
		{
			get { return m_bitMaskArray; }
		}

		public static char[] Digits
		{
			get { return m_digits; }
		}

		/// <summary>
		/// Vraci cast cisla na urcene pozici zprava. Napr: z 123456 pozice 2 vraci cislo 5
		/// </summary>
		/// <param name="number"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static int GetNumberOnPosition(int number, byte position)
		{
			string num = number.ToString(CultureInfo.InvariantCulture);
			num = num.Substring(num.Length - position, 1);
			return Convert.ToInt32(num);
		}

		/// <summary>
		/// Dekadicke cislo prevede na hexa reprezentaci
		/// </summary>
		/// <param name="numByte"></param>
		/// <returns>Textova reprezentace vstupniho parametru v sestnactkove soustave</returns>
		public static string DecToHex(int numByte)
		{
			return numByte.ToString("X");
		}

		/// <summary>
		/// Prevadi string zadane vstupni soustavy na int
		/// </summary>
		/// <param name="numString"></param>
		/// <param name="inputNumberSystem">Vstupni èíselná soustava. Možné hodnoty 2,10,16</param>
		/// <returns>Návratová hodnota èíslo typu INT</returns>
		public static int NumToDec(string numString, byte inputNumberSystem)
		{
			if (string.IsNullOrEmpty(numString)) throw new ArgumentNullException("numString", "Input string is null or empty");
			
			if (numString == "0") return 0;
			
			if (((inputNumberSystem != 16) && (inputNumberSystem != 2) && (inputNumberSystem != 10)))
				throw new ArgumentOutOfRangeException("inputNumberSystem", "Vstupní èíselná soustava musí mít v hodnoty 2,10,16");
			
			if ((numString.Length > 2) && (numString.Substring(0, 2) == HexPrefix) && (inputNumberSystem == 16))
				numString = numString.Substring(2, numString.Length - 2);

			int count;
			int result = 0;
			int x = 1;
			numString = numString.ToUpper();
			for (count = numString.Length - 1; count > -1; count--)
			{
				if (numString[count] != '0')
				{
					int w = Array.IndexOf(m_digits, numString[count]);
					if ((w >= inputNumberSystem) || (w == -1))
						throw new ArgumentOutOfRangeException("numString", "Input string invalid format.");
					result += (w * x);
				}
				x *= inputNumberSystem;
			}
			return result;
		}

		/// <summary>
		/// Ze vstupního Hexa øetezce vrací BIN øetezec 
		/// </summary>
		/// <param name="numString">øetìzec predstavujici hexa cislo. možný je formát s i bez prefixu(0x)</param>
		/// <returns>øetìzec obsahujici binarni reprezentaci parametru</returns>
		public static string HexToBin(string numString)
		{
			if (string.IsNullOrEmpty(numString)) throw new ArgumentNullException("numString", "Input string is null or empty");
			if (numString == "0") return numString;
			if ((numString.Length > 2) && (numString.Substring(0, 2) == HexPrefix))
				numString = numString.Substring(2, numString.Length - 2);

			StringBuilder s = new StringBuilder();
			int x;
			uint mocnina = 1;
			int num = NumToDec(numString, 16);
			for (x = 1; x < 32; x++)
			{
				mocnina *= 2;
			}
			while (mocnina > 0)
			{
				x = (int) (num / mocnina);
				s.Append(m_digits[x]);
				num %= (int) mocnina;
				mocnina /= 2;
			}
			return s.ToString().TrimStart(new[] {'0'});
		}

		/// <summary>
		/// Ze vstupniho cisla int vraci binarni reprezentaci. Druhy parametr udava delku Bin retezce
		/// </summary>
		/// <param name="num"></param>
		/// <param name="binaryLength"></param>
		/// <returns></returns>
		public static string IntToBin(int num, byte binaryLength)
		{
			StringBuilder s = new StringBuilder();
			int x;
			int mocnina = 1;
			for (x = 1; x < binaryLength; x++)
			{
				mocnina *= 2;
			}
			while (mocnina > 0)
			{
				x = num / mocnina;
				s.Append(m_digits[x]);
				num %= mocnina;
				mocnina /= 2;
			}
			return s.ToString();
		}

		/// <summary>
		/// Konvertuje int v BCD formatu na int.
		/// </summary>
		/// <param name="num">Cislo v BCD formatu</param>
		/// <returns></returns>
		public static int GetIntFromBCDFormat(int num)
		{
			return ((((num) >> 4) * 10) + ((num) & 0x0F));
		}

		/// <summary>
		/// Konvertuje cislo do BCD formatu
		/// </summary>
		/// <param name="num"></param>
		/// <returns>Cislo ve formatu BCD</returns>
		public static byte ConvertToBCDFormat(int num)
		{
			return (byte) (((num / 10) << 4) | ((num % 10)));
		}

		/// <summary>
		/// Vraci int vytvoreny s bytu. (low endian)
		/// </summary>
		/// <param name="highByte"></param>
		/// <param name="lowByte"></param>
		/// <returns></returns>
		public static int GetIntFromHighAndLowByte(byte highByte, byte lowByte)
		{
			return lowByte + (highByte << 8);
		}

		/// <summary>
		/// Prevede hexa retezec na pole bytu, za predpokladu, ze kazdy byte v retezci
		/// reprezentuji dva znaky (cisla mensi nez 0x10 musi mit tvar 00)
		/// </summary>
		/// <param name="hexaString">Vstupni hexa retezec</param>
		/// <returns>Pole bytu vytvorene ze vstupniho hexa retezce</returns>
		/// <exception cref="ArgumentException">Pokud delka retezce neni delitelna dvema nebo v pripade
		/// ze nektery znak v retezci neodpovida hexa ciselne reprezentaci</exception>
		/// <exception cref="ArgumentNullException">Pokud je vstupni retezec null nebo emty</exception>
		public static byte[] GetArrayFromHexaString(string hexaString)
		{
			if (string.IsNullOrEmpty(hexaString))
				throw new ArgumentNullException("hexaString", "Input hexa string is null or empty");
			if ((hexaString.Length % 2) > 0)
				throw new ArgumentException("Wrong input hexa string length", "hexaString");
			List<byte> result = new List<byte>(hexaString.Length / 2);
			for (int x = 0; x < hexaString.Length; x += 2)
			{
				try
				{
					result.Add(byte.Parse(hexaString.Substring(x, 2), NumberStyles.HexNumber));
				}
				catch (FormatException ex)
				{
					throw new ArgumentException(
						string.Format("Can't parse string '{0}' to byte. Position on string '{1}'", hexaString.Substring(x, 2), x),
						"hexaString", ex);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Vraci textovou reprezentaci pole bytu. Na kazdy byte v poli pripadaji dva znaky.
		/// </summary>
		/// <param name="data">pole bytu pro prevod na retezec</param>
		/// <returns>Textova hexa reprezentace pole bytu bez hex prefixu a oddelovace</returns>
		public static string GetHexaStringFromArray(byte[] data)
		{
			return GetHexaStringFromArray(data, string.Empty);
		}

		/// <summary>
		/// Vraci textovou reprezentaci pole bytu. Na kazdy byte v poli pripadaji dva znaky.
		/// Mezi jednotliva cisla vlozi oddelovac predany v argumentu.
		/// </summary>
		/// <param name="data">pole bytu pro prevod na retezec</param>
		/// <param name="separator">Oddelovac mezi jednotlivymi cisly</param>
		/// <returns>Textova hexa reprezentace pole bytu bez hex prefixu a oddelovace</returns>
		public static string GetHexaStringFromArray(byte[] data, string separator)
		{
			if (data.Length == 0)
				return string.Empty;
			StringBuilder str = new StringBuilder();
			for (int x = 0; x < data.Length; x++)
			{
				if (string.IsNullOrEmpty(separator))
					str.Append(data[x].ToString("X2"));
				else
					str.AppendFormat("{0}{1}", data[x].ToString("X2"), separator);
			}
			return str.ToString();
		}
		
		/// <summary>
		/// Mezi jednotlive znaky retezce vlozi separator.
		/// </summary>
		/// <param name="data">Vstupni retezec</param>
		/// <param name="countOfBytesBetweenSeparate">Pocet znaku mezi ktere bude vlozen separetor</param>
		/// <param name="separator">Oddelovac</param>
		/// <returns></returns>
		public static string SepareteBytesOnNumericString(string data, byte countOfBytesBetweenSeparate, string separator)
		{
			if (string.IsNullOrEmpty(data))
				return string.Empty;

			string result = data;
			for (int x = countOfBytesBetweenSeparate; x < data.Length; x += countOfBytesBetweenSeparate)
			{
				result = result.Insert(x + (separator.Length * (((x - countOfBytesBetweenSeparate) / countOfBytesBetweenSeparate))), separator);
			}

			return result;
		}

		/// <summary>
		/// Vrací true pokud je požadovaný bit 1
		/// </summary>
		/// <param name="inputBitMask">Vstupní bitová maska ve tvaru byte</param>
		/// <param name="requiredBit">Požadovaný bit v bitové masce indexovany od nuly (0 = prvni bit z prava)</param>
		/// <returns>Vrací true pokud požadovaný bit=1</returns>
		public static bool GetBit(byte inputBitMask, byte requiredBit)
		{
			return ((inputBitMask & m_bitMaskArray[requiredBit]) > 0);
		}
		
		
		/// <summary>
		/// Vraci pozici bitu z bitove masky.
		/// </summary>
		/// <param name="number">Vstupni bitova maska</param>
		/// <returns>Pozice v bitove masce indexovana od jednicky</returns>
		public static int GetBitPositionFromNumber(int number)
		{
			if (number == 1)
				return 1;

			if (GetCountOfSetBits(number) != 1)
				throw new ArgumentException("Number must by bit mask.", "number");
			
			int i = 1;
			while (number > 1)
			{
				number = number / 2;
				i++;
			}
			return i;
		}

        /// <summary>
        /// Spoèítá poèet nastavených bitù
        /// </summary>
        /// <param name="value">Vstupní byte hodnota</param>
        /// <returns>Poèet nastavených bitù</returns>
        public static int GetCountOfSetBits(byte value)
        {
            return GetCountOfSetBits(new BitArray(new[] {value}));
        }

        /// <summary>
        /// Spoèítá poèet nastavených bitù
        /// </summary>
        /// <param name="value">Vstupní int hodnota</param>
        /// <returns>Poèet nastavených bitù</returns>
        public static int GetCountOfSetBits(int value)
        {
            return GetCountOfSetBits(new BitArray(new[] {value}));
        }

        /// <summary>
        /// Spoèítá poèet nastavených bitù
        /// </summary>
        /// <param name="value">Vstupní long hodnota</param>
        /// <returns>Poèet nastavených bitù</returns>
        public static int GetCountOfSetBits(long value)
        {
            byte[] buf = new byte[8];
            buf[0] = (byte) value;
            buf[1] = (byte) (value >> 8);
            buf[2] = (byte) (value >> 16);
            buf[3] = (byte) (value >> 24);
            buf[4] = (byte) (value >> 32);
            buf[5] = (byte) (value >> 40);
            buf[6] = (byte) (value >> 48);
            buf[7] = (byte) (value >> 56);

            return GetCountOfSetBits(new BitArray(buf));
        }

        /// <summary>
        /// Spoèítá poèet nastavených bitù
        /// </summary>
        /// <param name="value">vstupní bitové pole</param>
        /// <returns>Poèet nastavených bitù</returns>
        public static int GetCountOfSetBits(BitArray value)
        {
        	return value.Cast<bool>().Count(bit => bit);
        }
	}
}