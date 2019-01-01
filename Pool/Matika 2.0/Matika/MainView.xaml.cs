using System.Windows;
using Matika;
using Matika_2._0;

namespace Matika
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }


        private void MainView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel dc)
            {
                dc.ResultTextBox = ResultTextBox;
            }
        }
    }
}