using System;
using System.Globalization;
using System.Windows.Data;

namespace DoubleTextBox
{
    public class CommaToDotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Rikam: pokud je v teto kulture oddelovac carka, replacni mi tecku za carku a naopak
            return culture.NumberFormat.CurrencyDecimalSeparator == "," ? value?.ToString().Replace('.', ',') : value?.ToString().Replace(',', '.');
        }
 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return culture.NumberFormat.CurrencyDecimalSeparator == "," ? value?.ToString().Replace('.', ',') : value?.ToString().Replace(',', '.');
        }
    }
}