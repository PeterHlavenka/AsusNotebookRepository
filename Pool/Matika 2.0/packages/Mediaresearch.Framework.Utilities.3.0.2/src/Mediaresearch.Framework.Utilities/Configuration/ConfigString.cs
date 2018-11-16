using System.Configuration;

namespace Mediaresearch.SimAdmin.Shared.Configuration
{
    public class ConfigString : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string) base["Name"]; }
            set
            {
                base["Name"] = value;
            }
        }
    }
}
