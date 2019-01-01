using System.Configuration;
using Mediaresearch.Framework.Utilities.Configuration;

namespace Matika.Configurations
{
    public class MatikaConfiguration : ConfigBase
    {
        public override string GetConfigName() => nameof(MatikaConfiguration);

        [ConfigurationProperty(nameof(Test), IsRequired = true)]
        public int Test => (int)this["Test"];

    }
}