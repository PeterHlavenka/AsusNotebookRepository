using System.Windows;
using WpfAnimatedGif;

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

        private void ShowingMonkey_OnAnimationLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel dc)
            {
                dc.MonkeyController = ImageBehavior.GetAnimationController(ShowingMonkey);
            }
        }
    }
}