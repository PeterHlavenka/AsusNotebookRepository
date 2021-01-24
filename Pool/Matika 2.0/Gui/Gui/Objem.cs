using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Matika.Gui
{
    public class Objem : PropertyChangedBase, IConvertable
    {
        private bool m_isEnabled;
        public int Step => 1000;
        public string Name => "Objem";
        public int MaxDifficulty { get; set; } = 50;
        public BitmapImage HelpImage => (BitmapImage) Application.Current.Resources["JednotkyObjemu"];


        public Dictionary<int, string> UnitsDictionary
        {
            get
            {
                var result = new Dictionary<int, string>
                {
                    {0, "mm³"},
                    {1, "cm³"},
                    {2, "dm³"},
                    {3, "m³"},
                    {4, string.Empty},
                    {5, string.Empty},
                    {6, "km³"}
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