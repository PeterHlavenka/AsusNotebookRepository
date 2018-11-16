using System;

namespace Mediaresearch.Framework.Utilities.MsSql.DataTypes
{
	public struct Timestamp : IConvertible
	{
		public const int TimestampLength = 8;
		public static readonly Timestamp Empty = new Timestamp(new byte[TimestampLength]);

		public Timestamp(byte[] value)
		{
			if(value == null)
				throw new ArgumentNullException("value");
			if(value.Length != TimestampLength)
				throw new ArgumentException("Length of value has to be exactly 8!", "value");
			m_value = new byte[TimestampLength];
			Array.Copy(value, 0, m_value, 0, TimestampLength);
		}

		private readonly byte[] m_value;

		private byte[] Value
		{
			get { return m_value ?? Empty.Value; }
		}

		public int this[int index]
		{
			get { return Value[index]; }
		}

		public byte[] ToByteArray()
		{
			byte[] result = new byte[TimestampLength];
			Array.Copy(Value, 0, result, 0, TimestampLength);
			return result;
		}

		public bool Equals(Timestamp other)
		{
			return Equals(other.Value, Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (Timestamp)) return false;
			return Equals((Timestamp) obj);
		}

		public override int GetHashCode()
		{
			return (Value != null ? Value.GetHashCode() : 0);
		}

		public TypeCode GetTypeCode()
		{
			return Type.GetTypeCode(typeof (Timestamp));
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public char ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public byte ToByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public short ToInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public int ToInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public long ToInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public float ToSingle(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public double ToDouble(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public string ToString(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			if (conversionType == typeof(byte[]))
				return ToByteArray();

			throw new NotImplementedException();
		}
	}
}