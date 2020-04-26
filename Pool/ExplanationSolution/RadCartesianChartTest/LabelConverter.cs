using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace RadCartesianChartTest
{
    public class LabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int.TryParse(value?.ToString(), out var bla );

            if (parameter is RadCartesianChart chart && bla != 0)
            {
                var context = chart.DataContext as DateTimeChartDataContext;
                
                var result = context?.AllData.FirstOrDefault(d => d.Value.Equals(bla));
                return result == null ?  "NULL" : result.ChannelName;               
            }
            return "to je v pici";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
