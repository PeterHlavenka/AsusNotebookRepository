using System.Collections.Generic;
using System.Linq;

namespace Matika.Gui
{
   public class UnitConversionsSettingsViewModel : SettingsBase
    {
        private bool m_decimalNumbers;
        private int m_stepDifference = 2;

        public UnitConversionsSettingsViewModel(IEnumerable<IConvertable> convertables)
        {
            Convertables = convertables;
            Convertables.FirstOrDefault().IsEnabled = true;
            DisplayName = "Nastavení";
        }

        public override int Difficulty
        {

            get => m_difficulty;
            set
            {
                m_difficulty = value;
                NotifyOfPropertyChange();
            }
        }

        public bool DecimalNumbers
        {
            get => m_decimalNumbers;
            set
            {
                m_decimalNumbers = value;
                NotifyOfPropertyChange();
            }
        }

        public int StepDifference
        {
            get => m_stepDifference;
            set
            {
                m_stepDifference = value;
                NotifyOfPropertyChange();
            }
        }
        
        public IEnumerable<IConvertable> Convertables { get; set; }
    }
}
