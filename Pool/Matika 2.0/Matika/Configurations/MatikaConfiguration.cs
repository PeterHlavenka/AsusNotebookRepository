using System.Configuration;
using Mediaresearch.Framework.Utilities.Configuration;

namespace Matika.Configurations
{
    public class MatikaConfiguration : ConfigBase
    {
        private readonly string m_name = "MatikaSection";

        public override string GetConfigName() => m_name;

        [ConfigurationProperty(nameof(StartDifficulty), IsRequired = true)]
        public int StartDifficulty => (int)this["StartDifficulty"];

    }
}