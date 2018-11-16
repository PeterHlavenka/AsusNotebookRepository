using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace TelerikGridViewExport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ObservableCollection<int> Motivlets = new ObservableCollection<int>{1,2};

        private void OnDistinctValuesLoading(object sender, GridViewDistinctValuesLoadingEventArgs e)
        {
            // This will make the grid display absolutely all distinct values for 
            // each column regardless of what filters might exist on other columns.

            var radGridView = sender as RadGridView;

            const bool filterDistinctValues = false;

            e.ItemsSource = radGridView?.GetDistinctValues(e.Column, filterDistinctValues);
        }
    }
}
