using System;
using System.Windows;
using System.Windows.Threading;

namespace CounterWithDependencyProperty
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty CounterProperty = DependencyProperty.Register("Counter", typeof(int), typeof(MainWindow), new PropertyMetadata(10)); // PropertyMetatata - sem se dava startovaci hodnota. Timer proto startuje od desitky
        // TOTO LZE I VE VIEWMODEL VIZ ADMIN REPRICING  // Podle Holubce pouzivat System.Threading.Timer viz poznamky DispatcherTimer.docx

        public MainWindow()
        {
            InitializeComponent();

            new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, // tady definuju v jakem intervalu se musi provadet callback
                delegate // toto je callback, ktery se vykona na threadu z threadPoolu kazdou sekundu. 
                {
                    int newValue;

                    if (Counter == 0)
                    {
                        newValue = 10;
                    }
                    else
                    {
                        newValue = Counter - 1;
                    }

                    SetValue(CounterProperty, newValue);
                }, Dispatcher
            );

            // Timer neni potreba startovat. 
        }


        public int Counter
        {
            get => (int) GetValue(CounterProperty);
            set => SetValue(CounterProperty, value);
        }
    }
}