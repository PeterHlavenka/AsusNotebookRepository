using System;
using System.Configuration;
using Mediaresearch.Framework.Domain.History;
using Mediaresearch.Framework.Utilities.Configuration;
using Mediaresearch.Framework.Utilities.Configuration.Sections;

namespace Mediaresearch.Framework.Utilities
{
    public class ProductionDateTimeSupport : IProductionDateTimeSupport
    {
        private readonly TimeSpan m_productionDayShift;

        public ProductionDateTimeSupport(IConfigurationProvider configurationProvider)
        {
            ProductionConfiguration config = configurationProvider.GetConfig<ProductionConfiguration>();

            if (config == null || string.IsNullOrEmpty(config.ProductionDay))
            {
                throw new SettingsPropertyNotFoundException("ProductionDay must be set in app.config.");
            }

            TimeSpan time;
            TimeSpan.TryParse(config.ProductionDay, out time);

            m_productionDayShift = time;
        }

        public DateTime GetProductionDay(DateTime date)
        {
            if (date.Year == HistoryConstants.DefaultValidTo.Year && date.Month == HistoryConstants.DefaultValidTo.Month && date.Date.Day == HistoryConstants.DefaultValidTo.Day)
            {
                return HistoryConstants.DefaultValidTo;
            }

            return date.Date.Add(GetProductionTime());
        }

        public TimeSpan GetProductionTime()
        {
            return m_productionDayShift;
        }
    }
}