using Caliburn.Micro;
using Matika.Settings;

namespace Matika
{
   public class SettingsBase : Screen, ISettings
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
