#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace ColorsComparer
{
    public class ColorToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            var stringColor = value?.ToString();

            return int.Parse(stringColor.Replace("#", ""), NumberStyles.HexNumber);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value == null) return null;

            var intColor = int.TryParse(value.ToString(), out var result);
            
            var color = System.Drawing.Color.FromArgb(result);
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}