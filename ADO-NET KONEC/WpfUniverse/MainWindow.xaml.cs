using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;

namespace WpfUniverse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public const string Conn = "Data Source=PHA0003;Initial Catalog=Vesmir;Integrated Security=True";
        
        public static GalaxyViewModel GalaxyViewModel { get; private set; }  //statickou promennou budem pouzivat vsude 
        public static PlanetsViewModel PlanetViewModel { get; private set; }

        public MainWindow()
        {
            GalaxyViewModel = new GalaxyViewModel();  //vytvorime instanci staticke promenne
            PlanetViewModel = new PlanetsViewModel(GalaxyViewModel);
            
            InitializeComponent();
            DataContext = this;

        }
       
    }
}
