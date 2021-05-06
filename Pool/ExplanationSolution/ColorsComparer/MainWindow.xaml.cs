using System;
using System.Collections;
using System.Reflection;
using System.Windows.Media;

namespace ColorsComparer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            ComboColor.ItemsSource = typeof(Colors).GetProperties();
            ComboColor1.ItemsSource = typeof(Colors).GetProperties();
        }
    }
}