using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mediaresearch.Framework.Utilities
{
	public static class BinaryFormatterHelper
	{
		/// <summary>
		/// Serializuje pole objektu T na pole bytu.
		/// </summary>
		/// <typeparam name="T">Libovolny object nebo struktura oznaceny atributem serializable</typeparam>
		/// <param name="array">Pole objekty T k serializaci</param>
		/// <returns>serializovana data</returns>
		public static byte[] Serialize<T>(T[] array)
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(buffer, array);

				buffer.Seek(0, SeekOrigin.Begin);
				return buffer.ToArray();
			}
		}

		/// <summary>
		/// Deserializuje z pole bytu pole objektu T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public static T[] Deserialize<T>(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				stream.Seek(0, SeekOrigin.Begin);
				BinaryFormatter serializer = new BinaryFormatter();
				return serializer.Deserialize(stream) as T[];
			}
		}

		public static byte[] SerializeSingleObject<T>(T o)
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(buffer, o);

				buffer.Seek(0, SeekOrigin.Begin);
				return buffer.ToArray();
			}			
		}

		public static T DeserializeSingleObject<T>(byte[] data)
			where T : class
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				stream.Seek(0, SeekOrigin.Begin);
				BinaryFormatter serializer = new BinaryFormatter();
				return serializer.Deserialize(stream) as T;
			}
		}

	}
}