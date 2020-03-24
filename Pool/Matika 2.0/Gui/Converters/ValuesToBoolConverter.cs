using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Matika.Converters
{
    public class ValuesToBoolConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var firstString = ((TextBox)values[0]).Text;
            var secondString = ((TextBox)values[1]).Text;

            if (string.IsNullOrWhiteSpace(firstString) || string.IsNullOrWhiteSpace(secondString))
            {
                return false;
            }

            var first = int.Parse(Regex.Replace(((TextBox)values[0]).Text, @"\s+", string.Empty)); 
            var sec = int.Parse(Regex.Replace(((TextBox)values[1]).Text, @"\s+", string.Empty));

            return first >= 1000 && sec >= 1000;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
