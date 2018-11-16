using System.Xml.Serialization;
using System.IO;

namespace Mediaresearch.Framework.Utilities
{
	public class XmlSerializerProvider
	{
		public static T Deserialize<T>(string inputXmlFile) where T:class
		{
			using (FileStream fs = new FileStream(inputXmlFile, FileMode.Open, FileAccess.Read))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(fs);
			}
		}
        
		public static T Deserialize<T>(Stream inputStream) where T : class
		{
			using (inputStream)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(inputStream);
			}
		}

        public static string SerializeObject<T>(T objectToSerialize)
        {
            var serializer = new XmlSerializer(typeof(T));
            var memStr = new MemoryStream();

            try
            {
                serializer.Serialize(memStr, objectToSerialize);
                memStr.Position = 0;

                return memStr.ToString();
            }
            finally
            {
                memStr.Close();
            }
        }
    }
}
