using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Matika.Gui
{
    public class Hmotnost : PropertyChangedBase, IConvertable
    {
        private bool m_isEnabled;
        public int Step => 10;
        public string Name => "Hmotnost";
        public int MaxDifficulty { get; set; } = 50;
        public BitmapImage HelpImage => (BitmapImage) Application.Current.Resources["JednotkyHmotnosti"];

        public Dictionary<int, string> UnitsDictionary
        {
            get
            {
                var result = new Dictionary<int, string>
                {
                    {0, "mg"},
                    {1, string.Empty},
                    {2, string.Empty},
                    {3, "g"},
                    {4, "dkg"},
                    {5, string.Empty},
                    {6, "kg"},
                    {7, string.Empty},
                    {8, string.Empty},
                    {9, "t"},
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