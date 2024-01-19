using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SfChartFontInReview
{
    class OffsetConverter : IMultiValueConverter
    {

        // kdyby to byl jen IValueConverter:
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double left = 0;
            if (double.TryParse(value?.ToString(), out var columnCount))
            {
                var chartWidth = 1000;

                if (columnCount > 0)
                {
                    left = (chartWidth / columnCount) / 2;
                    // remove 14% of left value
                    left = -(left - (left * 0.14));
                }
            }

            return new Thickness(left, 0, 0, 0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        //Multi:

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double left = 0;           
            if (double.TryParse(values[0]?.ToString(), out var columnCount) && double.TryParse(values[1]?.ToString(), out var plotOffset))
            {
                var chartWidth = 1000;

                if (columnCount > 0)
                {
                    left = (chartWidth / columnCount) / 2;
                    // remove 14% of left value
                    left = -(left - (left * 0.14));
                }
            }
           
                return new System.Windows.Thickness(left, 0, 0, 0);

     

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
