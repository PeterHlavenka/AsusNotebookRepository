using System.Globalization;

namespace Mediaresearch.Framework.Utilities.DateTimeFormat
{
	/// <summary>
	/// Pomocna trida pro nastaveni formatu datumu
	/// </summary>
	public static class DateTimeFormatHelper
	{
        private static string m_timeFormat; // HH:mm
        private static string m_longTimeFormat; // HH:mm:ss
		private static string m_dateFormat; // dd.MM.yyyy;
        private static string m_longDateFormat; // d. MMMM yyyy
		private static string m_shortDateTimeFormat; // dd.MM.yyyy HH:mm;
        private static string m_dateTimeFormat; // dd.MM.yyyy HH:mm:ss
        private static string m_fullDateTimeFormat; // d. MMMM yyyy HH:mm:ss;
        private static readonly string[] m_yearFormats = { ".yyyy", "/yyyy", " yyyy", "yyyy", ".YYYY", "/YYYY", " YYYY", "YYYY" };

        public static string TimeFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_timeFormat))
                {
                    return CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                }

                return m_timeFormat;
            }
            set
            {
                m_timeFormat = value;
            }
        }

        public static string LongTimeFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_longTimeFormat))
                {
                    return CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
                }

                return m_longTimeFormat;
            }
            set
            {
                m_longTimeFormat = value;
            }
        }

		/// <summary>
		/// Format datumu
		/// </summary>
		public static string DateFormat
		{
			get
			{
				if (string.IsNullOrEmpty(m_dateFormat))
				{
					return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
				}

				return m_dateFormat;
			}
			set 
			{
				m_dateFormat = value;
			}
		}

        public static string LongDateFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_longDateFormat))
                {
                    return CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
                }

                return m_longDateFormat;
            }
            set
            {
                m_longDateFormat = value;
            }
        }

        public static string DateTimeFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_dateTimeFormat))
                {
                    m_dateTimeFormat = $"{DateFormat} {LongTimeFormat}";
                }

                return m_dateTimeFormat;
            }
            set
            {
                m_dateTimeFormat = value;
            }
        }

		/// <summary>
        /// Format datumu s casem ve formatu d. MMMM yyyy HH:mm:ss
		/// </summary>
		public static string FullDateTimeFormat
		{
			get
			{
				if (string.IsNullOrEmpty(m_fullDateTimeFormat))
				{
					var formats = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns('G');
					if (formats.Length > 0)
						return formats[0];
				}

				return m_fullDateTimeFormat;
			}
			set
			{
				m_fullDateTimeFormat = value;
			}
		}

		/// <summary>
		/// Format datumu s casem ve formatu yyyy.mm.dd HH:mm
		/// </summary>
		public static string ShortDateTimeFormat
		{
			get
			{
				if (string.IsNullOrEmpty(m_shortDateTimeFormat))
				{
					var formats = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns('g');
					if (formats.Length > 0)
						return formats[0];
				}

				return m_shortDateTimeFormat;
			}
			set
			{
				m_shortDateTimeFormat = value;
			}
		}

        /// <summary>
		/// Format datumu s casem ve formatu dd.MM.yyyy HH:mm:ss.fff
		/// </summary>
		public static string MilisecondDateTimeFormat
        {
            get
            {
                string secondFormat = "ss";

                return DateTimeFormat.Replace(secondFormat, $"{secondFormat}.fff");
            }
        }

	    public static string ShortDateTimeFormatWithoutYear
        {
	        get
	        {

	            foreach (string yearFormat in m_yearFormats)
	            {
                    if (ShortDateTimeFormat.Contains(yearFormat))
                        return ShortDateTimeFormat.Replace(yearFormat, string.Empty);
                }

	            return ShortDateTimeFormat;
	        }
	    }

        public static string DateTimeFormatWithoutYear
        {
            get
            {
                foreach (string yearFormat in m_yearFormats)
                {
                    if (DateTimeFormat.Contains(yearFormat))
                        return DateTimeFormat.Replace(yearFormat, string.Empty);
                }

                return DateTimeFormat;
            }
        }
    }
}
