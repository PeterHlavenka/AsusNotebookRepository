using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using INotifyPropertyChanged.Annotations;

namespace INotifyPropertyChanged
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public MainWindow()
        {
            InitializeComponent();

            Context = new DataContext(RadGridView);   
            DataContext = Context;
        }

        private DataContext Context { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Context.Count++;
        }
    }
}
