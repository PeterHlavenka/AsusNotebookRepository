namespace Matika.Settings
{
   public class UnitConversionsSettingsViewModel : SettingsBase
    {
        private bool m_decimalNumbers;
        private int m_stepDifference = 1;

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
    }
}
