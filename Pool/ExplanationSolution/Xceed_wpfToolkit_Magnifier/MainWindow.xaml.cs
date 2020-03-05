using System.Windows;

namespace Xceed_wpfToolkit_Magnifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MyButton_OnClick(object sender, RoutedEventArgs e)
        {
            string text = " Bylo stlaceno tlacitko ";

            MyButton.Content = text != MyButton.Content.ToString() ? text : " Bylo znovu stlaceno tlacitko ";
        }
    }
}