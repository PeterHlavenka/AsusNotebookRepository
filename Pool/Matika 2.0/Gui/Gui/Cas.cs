using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Matika.Gui
{
    public class Cas : PropertyChangedBase, IConvertable
    {
        private bool m_isEnabled;
        public int Step => 60;
        public string Name => "Čas";
        public int MaxDifficulty { get; set; } = 5;
        public BitmapImage HelpImage => (BitmapImage) Application.Current.Resources["JednotkyCasu"];


        public Dictionary<int, string> UnitsDictionary
        {
            get
            {
                var result = new Dictionary<int, string>
                {
                    {0, "s"},
                    {1, "min"},
                    {2, "hod"},
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