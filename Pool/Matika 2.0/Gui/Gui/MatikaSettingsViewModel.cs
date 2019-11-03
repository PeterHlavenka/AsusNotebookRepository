using Caliburn.Micro;

namespace Matika.Gui
{
    public class MatikaSettingsViewModel : SettingsBase
    {
        private int m_addCount;
        private int m_differenceCount;
        private int m_divideCount;
        private int m_productCount;

        public int AddCount
        {
            get => m_addCount;
            set
            {
                m_addCount = value;
                NotifyOfPropertyChange();
            }
        }

        public int DifferenceCount
        {
            get => m_differenceCount;
            set
            {
                m_differenceCount = value;
                NotifyOfPropertyChange();
            }
        }

        public int ProductCount
        {
            get => m_productCount;
            set
            {
                m_productCount = value;
                NotifyOfPropertyChange();
            }
        }

        public int DivideCount
        {
            get => m_divideCount;
            set
            {
                m_divideCount = value;
                NotifyOfPropertyChange();
            }
        }
    }
}