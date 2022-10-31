#region

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace ColorsComparer
{
    public class ColorToGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush solidColorBrush)
            {
                return solidColorBrush.Color.G;
            }

            return null;
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