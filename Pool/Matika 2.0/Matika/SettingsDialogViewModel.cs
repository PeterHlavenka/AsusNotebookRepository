using Caliburn.Micro;

namespace Matika
{
    public class SettingsDialogViewModel : Screen
    {
        private int m_addCount;
        private int m_differenceCount;
        private int m_difficulty;
        private int m_divideCount;
        private int m_productCount;

        public int Difficulty
        {
            get => m_difficulty;
            set
            {
                m_difficulty = value;
                NotifyOfPropertyChange();
            }
        }

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