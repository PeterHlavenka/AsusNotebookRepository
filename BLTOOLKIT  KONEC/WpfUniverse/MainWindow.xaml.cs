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
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;
using System.Reflection;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using WpfUniverse.Properties;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;

namespace WpfUniverse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static VlastnostsManager Manager { get; private set; }
        public static GalaxyViewModel GalaxyViewModel { get; private set; }  //statickou promennou budem pouzivat vsude 
        public static PlanetsViewModel PlanetViewModel { get; private set; }
        public static PropertiesViewModel PropertiesViewModel { get; private set; }



        public MainWindow()
        {
            Assembly entities = Assembly.GetAssembly(typeof(Galaxie));


            EntityDaoFactory daoFactory = new EntityDaoFactory("universe", Settings.Default.ConnectionDb, new TransactionManager())
            {
                DaoAssemblies = new[] { entities.FullName },
                EnumTableAssemblies = new[] { entities.FullName },
            };

            IDaoSource daoSource = new EntityDaoFactoryDaoSource(daoFactory);
            
            GalaxyViewModel = new GalaxyViewModel(daoSource);
            Manager = new VlastnostsManager(daoSource);
            PlanetViewModel = new PlanetsViewModel(daoSource, GalaxyViewModel);
            PropertiesViewModel = new PropertiesViewModel(daoSource, PlanetViewModel);

            InitializeComponent();
            DataContext = this;
        }

    }
}
