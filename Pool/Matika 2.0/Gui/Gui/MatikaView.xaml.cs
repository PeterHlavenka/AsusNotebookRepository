using System.Windows;
using System.Windows.Controls;
using WpfAnimatedGif;

namespace Matika.Gui
{
    /// <summary>
    ///     Interaction logic for MatikaView.xaml
    /// </summary>
    public partial class MatikaView : UserControl
    {
        public MatikaView()
        {
            InitializeComponent();
        }

        private void MainView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MatikaViewModel dc)
            {
                dc.ResultTextBox = ResultTextBox;
            }
        }

        private void ShowingMonkey_OnAnimationLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MatikaViewModel dc)
            {
                dc.MonkeyController = ImageBehavior.GetAnimationController(ShowingMonkey);
                ResultTextBox.Focus();
            }
        }
    }
}