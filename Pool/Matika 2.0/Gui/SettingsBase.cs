using Caliburn.Micro;

namespace Matika
{
   public class SettingsBase : Screen
    {
        protected int m_difficulty;
        public virtual int Difficulty
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
