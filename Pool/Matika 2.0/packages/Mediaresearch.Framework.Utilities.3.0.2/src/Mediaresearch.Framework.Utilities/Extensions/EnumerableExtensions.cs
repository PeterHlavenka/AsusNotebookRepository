using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mediaresearch.Framework.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static string ToSeparatedString<T>(this IEnumerable<T> source, string separator)
        {
            if (separator == null)
            {
                separator = string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            foreach (T item in source)
            {
                if (item == null) continue;

                sb.Append(item);
                sb.Append(separator);
            }
            if (sb.Length >= (separator.Length + 1))
            {
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }

            return sb.ToString();
        }

        public static string ToCommaSeparatedString<T>(this IEnumerable<T> source)
        {
            return ToSeparatedString(source, ",");
        }

        public static string ToLineSeparatedString<T>(this IEnumerable<T> source)
        {
            return ToSeparatedString(source, Environment.NewLine);
        }

        public static double? Median<TColl, TValue>(this IEnumerable<TColl> source, Func<TColl, TValue> selector)
        {
            return source.Select(selector).Median();
        }

        public static double? Median<T>(
            this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null);

            int count = source.Count();
            if (count == 0)
                return null;

            source = source.OrderBy(n => n);

            int midpoint = count / 2;
            if (count % 2 == 0)
                return (Convert.ToDouble(source.ElementAt(midpoint - 1)) + Convert.ToDouble(source.ElementAt(midpoint))) / 2.0;

            return Convert.ToDouble(source.ElementAt(midpoint));
        }
    }
}
