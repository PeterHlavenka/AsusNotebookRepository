using System.Configuration;
using Mediaresearch.Framework.Utilities.Configuration;

namespace Matika.Configurations
{
    public class MatikaConfiguration : ConfigBase
    {
        private readonly string m_name = "MatikaSection";

        public override string GetConfigName() => m_name;
        //AddCount = addCount, DifferenceCount = differenceCount, ProductCount = productCount, DivideCount = di[ConfigurationProperty(nameof(StartDifficulty), IsRequired = true)]

        [ConfigurationProperty(nameof(StartDifficulty), IsRequired = true)]
        public int StartDifficulty => (int)this["StartDifficulty"];

        [ConfigurationProperty(nameof(AddCount), IsRequired = true)]
        public int AddCount => (int)this["AddCount"];

        [ConfigurationProperty(nameof(DifferenceCount), IsRequired = true)]
        public int DifferenceCount => (int)this["DifferenceCount"];

        [ConfigurationProperty(nameof(ProductCount), IsRequired = true)]
        public int ProductCount => (int)this["ProductCount"];

        [ConfigurationProperty(nameof(DivideCount), IsRequired = true)]
        public int DivideCount => (int)this["DivideCount"];
    }

    public class UnitConversionConfiguration : ConfigBase
    {
        private readonly string m_name = "UnitConversionSection";

        public override string GetConfigName() => m_name;

        [ConfigurationProperty(nameof(StartDifficulty), IsRequired = true)]
        public int StartDifficulty => (int)this["StartDifficulty"];

        [ConfigurationProperty(nameof(StepDifference), IsRequired = true)]
        public int StepDifference => (int) this["StepDifference"];
    }
}