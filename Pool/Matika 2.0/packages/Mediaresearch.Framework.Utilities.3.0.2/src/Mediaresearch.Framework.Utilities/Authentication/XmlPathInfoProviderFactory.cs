using System.IO;
using System.Reflection;
using System.Resources;

namespace Mediaresearch.Framework.Utilities.Authentication
{
	public class XmlPathInfoProviderFactory
	{
		private readonly string m_assemblyName;
		private readonly string m_resourcePath;

		public XmlPathInfoProviderFactory(string assemblyName, string resourcePath)
		{
			m_assemblyName = assemblyName;
			m_resourcePath = resourcePath;
		}

		public XMLPathInfoProvider Create()
		{
			Assembly typeAssembly = Assembly.Load(m_assemblyName);
			string embeddedResourcePath = string.Format("{0}.{1}", typeAssembly.GetName().Name, m_resourcePath);
			using (Stream resourceStream = typeAssembly.GetManifestResourceStream(embeddedResourcePath))
			{
				if(resourceStream == null)
					throw new MissingManifestResourceException(string.Format("Unable to find resource '{0}'! Is it really embedded resource?", embeddedResourcePath));
				XMLPathInfoProvider xmlPathInfoProvider = new XMLPathInfoProvider(resourceStream);
				return xmlPathInfoProvider;
			}
		}
	}
}