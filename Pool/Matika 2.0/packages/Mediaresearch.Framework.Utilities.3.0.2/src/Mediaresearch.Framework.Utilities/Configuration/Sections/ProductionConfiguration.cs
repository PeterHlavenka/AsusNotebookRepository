using System;
using System.Configuration;

namespace Mediaresearch.Framework.Utilities.Configuration.Sections
{
    public class ProductionConfiguration : ConfigBase
    {
        private object m_obj;
        //konstruktor je tu kvuli testu. nemelo by to tu byt, ale jak to jinak udelat?
        public ProductionConfiguration(string productionDay)
        {
            m_obj = productionDay;
        }

        public ProductionConfiguration()
        {
        }

        public override string GetConfigName()
        {
            return "ProductionConfiguration";
        }

        [ConfigurationProperty("ProductionDay")]
        public string ProductionDay
        {
            get
            {
                m_obj = m_obj ?? this["ProductionDay"];
                return (string)m_obj;
            }
        }

        public TimeSpan GetProductionDayAsTimeSpan()
        {
            return TimeSpan.Parse(ProductionDay);
        }
    }
}