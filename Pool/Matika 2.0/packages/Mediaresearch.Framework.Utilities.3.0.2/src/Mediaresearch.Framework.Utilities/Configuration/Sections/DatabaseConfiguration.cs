using System.Configuration;
using Mediaresearch.Framework.Utilities.Configuration.Elements;

namespace Mediaresearch.Framework.Utilities.Configuration.Sections
{
    public class DatabaseConfiguration : ConfigBase
    {
        private const string ConfigName = "DatabaseConfiguration";

        public override string GetConfigName() { return ConfigName; }

        public static DatabaseConfiguration GetConfig()
        {
            var section = ConfigurationManager.GetSection(ConfigName) as DatabaseConfiguration;
            return section ?? new DatabaseConfiguration();
        }

        [ConfigurationProperty("PoolingDaoSource")]
        public DaoSourceConfig PoolingDaoSource
        {
            get
            {
                object obj = this["PoolingDaoSource"];
                return obj as DaoSourceConfig;
            }
        }

        [ConfigurationProperty("ShodanDaoSource")]
        public DaoSourceConfig ShodanDaoSource
        {
            get
            {
                object obj = this["ShodanDaoSource"];
                return obj as DaoSourceConfig;
            }
        }

        [ConfigurationProperty("DifferentDatabases", IsRequired = false)]
        public bool DifferentDatabases
        {
            get
            {
                return (bool) this["DifferentDatabases"];
            }
        }

        [ConfigurationProperty("CommonDaoSource")]
        public DaoSourceConfig CommonDaoSource
        {
            get
            {
                object obj = this["CommonDaoSource"];
                return obj as DaoSourceConfig;
            }
        }
    }
}
