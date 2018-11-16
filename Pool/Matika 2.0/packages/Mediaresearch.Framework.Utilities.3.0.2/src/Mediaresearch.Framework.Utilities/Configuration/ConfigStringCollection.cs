using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Mediaresearch.SimAdmin.Shared.Configuration;

namespace Mediaresearch.Framework.Utilities.Configuration
{
    public class ConfigStringCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigString();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigString)element).Name;
        }

        public string[] ToArray()
        {
            return this.Cast<ConfigString>().Select(x => x.Name).ToArray();
        }

        public List<string> ToList()
        {
            return this.Cast<ConfigString>().Select(x => x.Name).ToList();
        }
    }
}
