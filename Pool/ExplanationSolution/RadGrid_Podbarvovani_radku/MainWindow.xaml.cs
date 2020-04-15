using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace RadGrid_Podbarvovani_radku
{
   // MEDIARESEARCH.GUI.TELERIK JE NATVRDO DO POOLU
    public partial class MainWindow  
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Person> persons = new List<Person>{new Person("Peter", "Hlavenka"), new Person("Vladimira", "Hlavenkova")};
            RadGridView.ItemsSource = persons;
        }

       
        
        private void RadGridView_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (e.Row is GridViewHeaderRow)
            {
                e.Row.MinHeight = 35;
                var headerCell = e.Row.ChildrenOfType<Grid>().FirstOrDefault(x => x.Name == "PART_HeaderCellGrid");
            }
        }

        private void RadGridView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (e.Source is RadGridView grid)
            {
                var neco = grid.ChildrenOfType<GridViewRow>();
                var necofi = neco.FirstOrDefault(x => x.Name == "Background_Over");
            }
        }
    }

    public class Person
    {
        public Person(string name, string surName)
        {
            Name = name;
            SurName = surName;
        }

        public string Name { get; set; }
        public string SurName { get; set; }
    }
}