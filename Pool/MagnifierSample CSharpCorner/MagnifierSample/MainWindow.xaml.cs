using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagnifierSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ZoomFactorDropDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MagnifiyingGlass.ZoomFactor = (double)ZoomFactorDropDown.Value;
        }

        private void RadiusDropDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MagnifiyingGlass.Radius = (double)RadiusDropDown.Value;
         }

        private void EnableMagnifier_Checked(object sender, RoutedEventArgs e)
        {
             MagnifiyingGlass.Visibility = Visibility.Visible;   
        }

        private void EnableMagnifier_Unchecked(object sender, RoutedEventArgs e)
        {
            MagnifiyingGlass.Visibility = Visibility.Hidden;   
        }       
      
    }
}
