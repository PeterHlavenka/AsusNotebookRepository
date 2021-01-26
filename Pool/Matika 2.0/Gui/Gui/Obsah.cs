using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Matika.Gui
{
    public class Obsah : PropertyChangedBase, IConvertable
    {
        private bool m_isEnabled;
        public int Step => 100;
        public string Name => "Obsah";
        public int MaxDifficulty { get; set; } = 50;
        public BitmapImage HelpImage => (BitmapImage) Application.Current.Resources["JednotkyObsahu"];


        public Dictionary<int, string> UnitsDictionary
        {
            get
            {
                var result = new Dictionary<int, string>
                {
                    {0, "mm²"},
                    {1, "cm²"},
                    {2, "dm²"},
                    {3, "m²"},
                    {4, string.Empty},
                    {5, string.Empty},
                    {6, "km²"}
                };
                return result;
            }
        }

        public bool IsEnabled
        {
            get => m_isEnabled;
            set
            {
                m_isEnabled = value; 
                NotifyOfPropertyChange();
            }
        }
    }
}