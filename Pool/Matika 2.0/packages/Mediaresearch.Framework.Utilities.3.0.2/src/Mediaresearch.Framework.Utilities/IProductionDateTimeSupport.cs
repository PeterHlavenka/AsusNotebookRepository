using System;

namespace Mediaresearch.Framework.Utilities
{
    public interface IProductionDateTimeSupport
    {
        DateTime GetProductionDay(DateTime date);

        TimeSpan GetProductionTime();
    }
}