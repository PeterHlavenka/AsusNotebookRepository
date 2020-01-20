using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ResizePopup
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowPopup(object sender, RoutedEventArgs e)

        {
            Popup.IsOpen = true;
        }


        private void onDragStarted(object sender, DragStartedEventArgs e)

        {
            var t = (Thumb) sender;

            t.Cursor = Cursors.Hand;
        }


        private void onDragDelta(object sender, DragDeltaEventArgs e)

        {
            var yadjust = Popup.Height + e.VerticalChange;

            var xadjust = Popup.Width + e.HorizontalChange;

            if (xadjust >= 0 && yadjust >= 0)

            {
                Popup.Width = xadjust;

                Popup.Height = yadjust;
            }
        }


        private void onDragCompleted(object sender, DragCompletedEventArgs e)

        {
            var t = (Thumb) sender;

            t.Cursor = null;
        }
    }
}