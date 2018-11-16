using System.Globalization;

namespace Mediaresearch.Framework.Utilities.DateTimeFormat.Services
{
    public interface IDateTimeFormatService
    {
        void Initialize(string dateFormat, string longDateFormat, string timeFormat, string longTimeFormat, CultureInfo cultureInfo);

        string DateFormat { get; }

        string LongDateFormat { get; }

        string TimeFormat { get; }

        string LongTimeFormat { get; }

        string DateTimeFormat { get; }

        string ShortDateTimeFormat { get; }

        string FullDateTimeFormat { get; }

        string MilisecondDateTimeFormat { get; }
    }
}
