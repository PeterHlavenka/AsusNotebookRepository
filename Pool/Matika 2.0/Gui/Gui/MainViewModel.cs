using Caliburn.Micro;

namespace Matika.Gui
{
    public class MainViewModel : Screen
    {
        //public MainViewModel(MatikaViewModel matikaViewModel, EnumeratedWordsViewModel enumeratedWordsViewModel, UnitConversionViewModel unitConversionViewModel)
        //{
        //    MatikaViewModel = matikaViewModel;
        //    EnumeratedWordsViewModel = enumeratedWordsViewModel;
        //    UnitConversionViewModel = unitConversionViewModel;
        //}

        public MainViewModel(MatikaViewModel matikaViewModel, UnitConversionViewModel unitConversionViewModel)
        {
            MatikaViewModel = matikaViewModel;           
            UnitConversionViewModel = unitConversionViewModel;
        }

        public MatikaViewModel MatikaViewModel { get; set; }
        public EnumeratedWordsViewModel EnumeratedWordsViewModel { get; set; }
        public UnitConversionViewModel UnitConversionViewModel { get; set; }
    }
}