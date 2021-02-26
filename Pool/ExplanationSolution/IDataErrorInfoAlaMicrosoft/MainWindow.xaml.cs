using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IDataErrorInfoAlaMicrosoft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LayoutRoot.DataContext = new Product() { Id = 10, Name = "food" };
        }
        
        // Commits text box values when the user presses ENTER. This makes it 
        // easier to experiment with different values in the text boxes.
        private void TextBox_KeyDown(object sender,
            System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) (sender as TextBox)
                .GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void DataField_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter) (sender as TextBox)
                .GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private static readonly Brush _errorBrush = new SolidColorBrush(Colors.Red);

        private void OnBindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            TextBox dataBox = sender as TextBox;
            StackPanel dataPanel = dataBox.Parent as StackPanel;
            TextBlock errorBlock = dataPanel.Children.OfType<TextBlock>().Single();

            if (e.Action == ValidationErrorEventAction.Added)
            {
                dataBox.BorderBrush = _errorBrush;
                errorBlock.Text = e.Error.ErrorContent.ToString();
            }
            else
            {
                dataBox.BorderBrush = (Brush)Resources["PhoneBackgroundBrush"];
                errorBlock.Text = string.Empty;
            }
        }
    }
}