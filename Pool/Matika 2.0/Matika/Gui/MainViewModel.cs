using Caliburn.Micro;

namespace Matika.Gui
{
    public class MainViewModel : Screen
    {
        public MainViewModel(MatikaViewModel matikaViewModel, EnumeratedWordsViewModel enumeratedWordsViewModel)
        {
            MatikaViewModel = matikaViewModel;
            EnumeratedWordsViewModel = enumeratedWordsViewModel;                        
        }

        public MatikaViewModel MatikaViewModel { get; set; }
        public EnumeratedWordsViewModel EnumeratedWordsViewModel { get; set; }
    }
}