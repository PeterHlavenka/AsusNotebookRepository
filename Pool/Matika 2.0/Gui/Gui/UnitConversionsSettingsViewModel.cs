namespace Matika.Gui
{
   public class UnitConversionsSettingsViewModel : SettingsBase
    {
        private bool m_decimalNumbers;

        public bool DecimalNumbers
        {
            get => m_decimalNumbers;
            set
            {
                m_decimalNumbers = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
