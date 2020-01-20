using System.Windows;

namespace INotifyPropertyChanged
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Context = new DataContext(RadGridView);
            DataContext = Context;
        }

        private DataContext Context { get; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Context.Count++;
        }
    }
}