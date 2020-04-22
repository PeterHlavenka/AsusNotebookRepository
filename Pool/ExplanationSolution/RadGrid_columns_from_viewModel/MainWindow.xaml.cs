using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Common;
using Telerik.Windows.Controls;

namespace RadGrid_columns_from_viewModel
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Persons = new List<Person>{new Person("Peter", "Hlavenka"), new Person("Vladimira", "Hlavenkova"), new Person("Renata", "Hlavenkova"), new Person("Tatiana", "Hlavenkova") };
            RadGridView.ItemsSource = Persons;
            RadGridView.AutoGenerateColumns = false;

            List<GridViewColumn> columns = new List<GridViewColumn>
            {
                new GridViewDataColumn {DataMemberBinding = new Binding("Name"), Header = "My first Column"}, 
                new GridViewDataColumn {DataMemberBinding = new Binding("SurName"), Header = "My second Column"}
            };
            
            RadGridView.Columns.AddRange(columns);
        }
        private List<Person> Persons { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var myColumn = RadGridView.Columns["MyColumn"];  // bud takhle nebo:
            myColumn = RadGridView.Columns[1];
            myColumn.Header = "test";
            Persons.Last().SurName = "test";
            RadGridView.Rebind();
        }
    }
}