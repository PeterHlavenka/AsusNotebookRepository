using Caliburn.Micro;

namespace Matika
{
   public class SettingsBase : Screen
    {
        private int m_difficulty;
        public int Difficulty
        {
            get => m_difficulty;
            set
            {
                m_difficulty = value;
                NotifyOfPropertyChange();
            }
        }

    }
}
