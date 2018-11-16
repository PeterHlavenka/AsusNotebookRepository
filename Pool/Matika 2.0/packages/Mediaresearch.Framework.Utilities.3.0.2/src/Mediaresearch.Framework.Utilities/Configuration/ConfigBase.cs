using System.Configuration;

namespace Mediaresearch.Framework.Utilities.Configuration
{
    public abstract class ConfigBase : ConfigurationSection
    {
        public abstract string GetConfigName();
    }
}
