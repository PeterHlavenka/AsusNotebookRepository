using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Mediaresearch.Framework.Utilities.Authentication
{
	/// <summary>
	/// XMLPathInfoProvider je tu protoze se mi zatim nechce delat nova tabulka do db. Az ji nekdo udela, implementujte prosim metody DatabasePathInfoProvider.
	/// </summary>
	public class XMLPathInfoProvider : IPathInfoProvider
	{
		private readonly List<PathInfo> m_pathInfoCache = new List<PathInfo>();

		public XMLPathInfoProvider(Stream configurationFileStream)
		{
			Configure(configurationFileStream);
		}

		public XMLPathInfoProvider(string configurationFilePath)
		{
			using (Stream stream = File.OpenRead(configurationFilePath))
			{
				Configure(stream);
			}
		}

		private void Configure(Stream configurationFileStream)
		{
			m_pathInfoCache.Clear();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(configurationFileStream);
			XmlNodeList xmlElements = xmlDocument.GetElementsByTagName("pathInfo");
			foreach (XmlNode node in xmlElements)
			{
				XmlAttributeCollection attributes = node.Attributes;
				XmlNode credentialsNode = node["credentials"];

				Credentials credentials = credentialsNode == null ? null : new Credentials(credentialsNode.Attributes[0].Value, credentialsNode.Attributes[1].Value);
				m_pathInfoCache.Add(new PathInfo(attributes[0].Value, credentials));
			}
		}

		public List<PathInfo> GetPathInfos()
		{
			return m_pathInfoCache;
		}

		public PathInfo GetPathInfo(string path)
		{
			if (path == null)
				return null;

			if (string.IsNullOrEmpty(path.Trim()))
				return new PathInfo(path, null);

			List<PathInfo> possiblePathInfos = new List<PathInfo>();
			string fullPath = Path.GetFullPath(path);

			foreach (PathInfo possiblePathInfo in m_pathInfoCache)
			{
				string directory = Path.GetFullPath(possiblePathInfo.Path);
				if(fullPath.Contains(directory))
					possiblePathInfos.Add(possiblePathInfo);
			}

			if (!possiblePathInfos.Any())
				return null;

			int maxPathLength = possiblePathInfos.Max(pi => pi.Path.Length);
			return possiblePathInfos.Where(pi => pi.Path.Length == maxPathLength).FirstOrDefault();
		}
	}
}
