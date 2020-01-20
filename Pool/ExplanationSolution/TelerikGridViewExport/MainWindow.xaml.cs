using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace TelerikGridViewExport
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<int> Motivlets = new ObservableCollection<int> {1, 2};

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

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