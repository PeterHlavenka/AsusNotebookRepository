using System.Configuration;
using Mediaresearch.SimAdmin.Shared.Configuration;

namespace Mediaresearch.Framework.Utilities.Configuration.Elements
{
    public class DaoSourceConfig : ConfigurationElement
    {
        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["ConnectionString"] as string;
            }
        }

        [ConfigurationProperty("DbAlias", IsRequired = true)]
        public string DbAlias
        {
            get
            {
                return this["DbAlias"] as string;
            }
        }

        [ConfigurationProperty("ServerTimeZone", IsRequired = true)]
        public string ServerTimeZone
        {
            get
            {
                return this["ServerTimeZone"] as string;
            }
        }

        [ConfigurationProperty("WorkableSchemas", IsRequired = true)]
        [ConfigurationCollection(typeof(ConfigString), AddItemName = "Item")]
        public ConfigStringCollection WorkableSchemas
        {
            get
            {
                return this["WorkableSchemas"] as ConfigStringCollection;
            }
        }

        [ConfigurationProperty("DaoAssemblies", IsRequired = true)]
        [ConfigurationCollection(typeof(ConfigString), AddItemName = "Item")]
        public ConfigStringCollection DaoAssemblies
        {
            get
            {
                return this["DaoAssemblies"] as ConfigStringCollection;
            }
        }

        [ConfigurationProperty("EnumTableAssemblies", IsRequired = true)]
        [ConfigurationCollection(typeof(ConfigString), AddItemName = "Item")]
        public ConfigStringCollection EnumTableAssemblies
        {
            get
            {
                return this["EnumTableAssemblies"] as ConfigStringCollection;
            }
        }

		[ConfigurationProperty("EditingHintsAssemblies", IsRequired = true)]
		[ConfigurationCollection(typeof(ConfigString), AddItemName = "Item")]
		public ConfigStringCollection EditingHintsAssemblies
		{
			get { return this["EditingHintsAssemblies"] as ConfigStringCollection; }
		}

        [ConfigurationProperty("DefaultUsername", IsRequired = false)]
        public string DefaultUsername
        {
            get
            {
                return this["DefaultUsername"] as string;
            }
        }

    }
}
