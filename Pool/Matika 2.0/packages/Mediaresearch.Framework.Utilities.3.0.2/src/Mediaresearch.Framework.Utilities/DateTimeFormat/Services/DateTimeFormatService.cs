using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using log4net;

namespace Mediaresearch.Framework.Utilities.DateTimeFormat.Services
{
    public class DateTimeFormatService : IDateTimeFormatService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string m_dateFormat; // dd.MM.yyyy
        private string m_longDateFormat; // d. MMMM yyyy
        private string m_timeFormat; // HH:mm
        private string m_longTimeFormat; // HH:mm:ss
        private string m_dateTimeFormat; // dd.MM.yyyy HH:mm:ss
        private string m_shortDateTimeFormat; // dd.MM.yyyy HH:mm
        private string m_fullDateTimeFormat; // d. MMMM yyyy HH:mm:ss
        private string m_milisecondDateTimeFormat; // dd.MM.yyyy HH:mm:ss.fff

        private CultureInfo m_cultureInfo;

        public string DateFormat 
        {
            get { return string.IsNullOrEmpty(m_dateFormat) ? CultureDateTimeFormatInfo.ShortDatePattern : m_dateFormat; }
        }

        public string LongDateFormat 
        {
            get { return string.IsNullOrEmpty(m_longDateFormat) ? CultureDateTimeFormatInfo.LongDatePattern : m_longDateFormat; }
        }

        public string TimeFormat
        {
            get { return string.IsNullOrEmpty(m_timeFormat) ? CultureDateTimeFormatInfo.ShortTimePattern : m_timeFormat; }
        }

        public string LongTimeFormat 
        {
            get { return string.IsNullOrEmpty(m_longTimeFormat) ? CultureDateTimeFormatInfo.LongTimePattern : m_longTimeFormat; }
        }

        public string DateTimeFormat
        {
            get { return string.IsNullOrEmpty(m_dateTimeFormat) ? $"{DateFormat} {LongTimeFormat}" : m_dateTimeFormat; }
        }

        public string ShortDateTimeFormat 
        {
            get { return string.IsNullOrEmpty(m_shortDateTimeFormat) ? GetShortDateTimeFormat() : m_shortDateTimeFormat; }
        }

        public string FullDateTimeFormat
        {
            get { return string.IsNullOrEmpty(m_fullDateTimeFormat) ? GetFullDateTimeFormat() : m_fullDateTimeFormat; }
        }

        public string MilisecondDateTimeFormat
        {
            get { return string.IsNullOrEmpty(m_milisecondDateTimeFormat) ? GetMilisecondDateTimeFormat() : m_milisecondDateTimeFormat; }
        }

        public void Initialize(string dateFormat, string longDateFormat, string timeFormat, string longTimeFormat, CultureInfo cultureInfo)
        {
            m_dateFormat = dateFormat;
            m_longDateFormat = longDateFormat;
            m_timeFormat = timeFormat;
            m_longTimeFormat = longTimeFormat;
            m_cultureInfo = cultureInfo;

            try
            {
                if (!string.IsNullOrWhiteSpace(m_dateFormat))
                    cultureInfo.DateTimeFormat.ShortDatePattern = m_dateFormat;

                if (!string.IsNullOrWhiteSpace(m_longDateFormat))
                    cultureInfo.DateTimeFormat.LongDatePattern = m_longDateFormat;

                if (!string.IsNullOrWhiteSpace(m_timeFormat))
                    cultureInfo.DateTimeFormat.ShortTimePattern = m_timeFormat;

                if (!string.IsNullOrWhiteSpace(m_longTimeFormat))
                    cultureInfo.DateTimeFormat.LongTimePattern = m_longTimeFormat;

                if (!string.IsNullOrWhiteSpace(m_dateFormat) && !string.IsNullOrWhiteSpace(m_timeFormat))
                    m_shortDateTimeFormat = $"{m_dateFormat} {m_timeFormat}";

                if (!string.IsNullOrWhiteSpace(m_longDateFormat) && !string.IsNullOrWhiteSpace(m_longTimeFormat))
                    m_fullDateTimeFormat = $"{m_longDateFormat} {m_longTimeFormat}";

                if (!string.IsNullOrWhiteSpace(m_dateFormat) && !string.IsNullOrWhiteSpace(m_longTimeFormat))
                    m_dateTimeFormat = $"{m_dateFormat} {m_longTimeFormat}";
            }
            catch (Exception ex)
            {
                if (m_log.IsErrorEnabled)
                    m_log.Error(ex);
            }
        }

        private DateTimeFormatInfo CultureDateTimeFormatInfo
        {
            get { return m_cultureInfo?.DateTimeFormat ?? Thread.CurrentThread.CurrentCulture.DateTimeFormat; }
        }

        private string GetShortDateTimeFormat()
        {
            return GetShortDateTimeFormat(Thread.CurrentThread.CurrentCulture);
        }

        private string GetShortDateTimeFormat(CultureInfo cultureInfo)
        {
            var formats = cultureInfo.DateTimeFormat.GetAllDateTimePatterns('g');
            if (formats.Length > 0)
            {
                return formats[0];
            }

            return string.Empty;
        }

        private string GetFullDateTimeFormat()
        {
            return GetFullDateTimeFormat(Thread.CurrentThread.CurrentCulture);
        }

        private string GetMilisecondDateTimeFormat()
        {
            string secondFormat = "ss";

            return DateTimeFormat.Replace(secondFormat, $"{secondFormat}.fff");
        }

        private string GetFullDateTimeFormat(CultureInfo cultureInfo)
        {
            var formats = cultureInfo.DateTimeFormat.GetAllDateTimePatterns('G');
            if (formats.Length > 0)
            {
                return formats[0];
            }

            return string.Empty;
        }
    }
}
