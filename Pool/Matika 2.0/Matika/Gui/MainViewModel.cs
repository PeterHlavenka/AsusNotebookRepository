using Caliburn.Micro;

namespace Matika.Gui
{
    public class MainViewModel : Screen
    {
        private MatikaViewModel m_matikaViewModel;

        public MainViewModel(MatikaViewModel matikaViewModel)
        {
            MatikaViewModel = matikaViewModel;
            EnumeratedWordsViewModel = new EnumeratedWordsViewModel();

            
            
        }

        public MatikaViewModel MatikaViewModel
        {
            get => m_matikaViewModel;
            set
            {
                m_matikaViewModel = value;
                NotifyOfPropertyChange();
            }
        }
       public EnumeratedWordsViewModel EnumeratedWordsViewModel { get; set; }
    }
}