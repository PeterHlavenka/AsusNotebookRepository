using System.Collections.Generic;
using System.Linq;

namespace Matika.Gui
{
   public class UnitConversionsSettingsViewModel : SettingsBase
    {
        private bool m_decimalNumbers;
        private int m_stepDifference = 3;

        public UnitConversionsSettingsViewModel(IEnumerable<IConvertable> convertables)
        {
            Convertables = convertables;
            Convertables.FirstOrDefault().IsEnabled = true;
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
