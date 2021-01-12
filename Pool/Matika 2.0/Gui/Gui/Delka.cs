using System.Collections.Generic;
using Caliburn.Micro;

namespace Matika.Gui
{
    public class Delka : PropertyChangedBase, IConvertable
    {
        private bool m_isEnabled;
        public int Step => 10;
        public string Name => "Délka";
        public int MaxDifficulty { get; set; } = 100;

        public Dictionary<int, string> UnitsDictionary
        {
            get
            {
                var result = new Dictionary<int, string>
                {
                    {0, "mm"},
                    {1, "cm"},
                    {2, "dm"},
                    {3, "m"},
                    {4, string.Empty},
                    {5, string.Empty},
                    {6, "km"}
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