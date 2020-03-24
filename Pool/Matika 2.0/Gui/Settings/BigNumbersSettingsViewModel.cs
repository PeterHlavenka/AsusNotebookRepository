using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Matika.Settings
{
    public class BigNumbersSettingsViewModel : SettingsBase
    {
        public BigNumbersSettingsViewModel(int first, int second)
        {
            FirstNumberSize = first;
            SecondNumberSize = second;
        }

        public int FirstNumberSize { get; set; }
        public int SecondNumberSize { get; set; }
        public bool CanWholeThousand => FirstNumberSize > 1000 && SecondNumberSize > 1000;
        public bool WholeThousands { get; set; }
        public bool Overlaps { get; set; }

        public void FirstTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                var text = Regex.Replace(tb.Text, @"\s+", string.Empty);

                if ( int.TryParse(text, out var number))
                {
                    FirstNumberSize = number;
                    NotifyOfPropertyChange(() =>(FirstNumberSize));
                }
            }
        }

        public void SecondTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                var text = Regex.Replace(tb.Text, @"\s+", string.Empty);

                if (int.TryParse(text, out var number))
                {
                    SecondNumberSize = number;
                    NotifyOfPropertyChange(() => (SecondNumberSize));
                }
            }
        }
    }
}