using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.Views;



namespace WpfUniverse.ViewModels
{
    public class PlanetsViewModel : INotifyPropertyChanged, IPlanetSelector
    {
        private IPlanetDao m_planetDao;
        private IGalaxySelector m_GalaxySelector;
        private ObservableCollection<PlanetDataContract> m_ListOfPlanetsFromSelectedGalaxies;             //PRETYPOVANO Z OBSERVABLECOLLECTION
        private GalaxyDataContract m_selectedGalaxy;
        private PlanetDataContract m_selectedPlanet;
        private IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        private IEventRegistrator m_eventRegistrator;
        private IDaoSource m_daoSource;

        public PlanetsViewModel(IDaoSource daoSource, IGalaxySelector selector)   
        {
            m_planetDao = daoSource.GetDaoByEntityType<IPlanetDao, Planeta,int>(); //nacte planety 
            m_vlastnostiPlanetDao = daoSource.GetMultiKeyDaoByEntityType<IVlastnostiPlanetDao, VlastnostiPlanet>();
            m_GalaxySelector = selector;  //pridal jsem privatni atribut abych se dostal k vlastnostem GalaxyVM
            m_GalaxySelector.OnGalaxyChanged += OnGalaxyChanged;  //registrace posluchace
            m_daoSource = daoSource;  // predava se tride EditPropertyViewModel pri vytvareni okna v metode DoEditProperties

            ListOfPlanetsFromSelectedGalaxies = new ObservableCollection<PlanetDataContract>();
            
            AddPlanet = new CommandBase(() => m_selectedGalaxy != null, DoAddPlanet);
            RemovePlanet = new CommandBase(CheckNullPlanet, DoRemovePlanet);
            EditPlanet = new CommandBase(CheckNullPlanet, DoEditPlanet);
            EditProperties = new CommandBase(CheckNullPlanet, DoEditProperties);
        }



        //COMMANDY
        public CommandBase AddPlanet { get; private set; }
        public CommandBase RemovePlanet { get; private set; }
        public CommandBase EditPlanet { get; private set; }
        public CommandBase EditProperties { get; private set; }

        /// <summary>
        /// Vklada zavislost ne do konstruktoru ale do promenne m_propertiesViewModel. Ta se pak pouziva v metode DoEditProperties kde se vklada do EditPropertyViewModelu 
        /// </summary>
        /// <param name="propertiesViewModel"></param>
        public void SetPropertiesDependency(IEventRegistrator registrator)
        {
            m_eventRegistrator = registrator;
        }

        //VLASTNOSTI
        public ObservableCollection<PlanetDataContract> ListOfPlanetsFromSelectedGalaxies
        {
            get { return m_ListOfPlanetsFromSelectedGalaxies; }
            set { m_ListOfPlanetsFromSelectedGalaxies = value; OnPropertyChanged(nameof(ListOfPlanetsFromSelectedGalaxies)); }
        }

        public PlanetDataContract SelectedPlanet
        {
            get { return m_selectedPlanet; }
            set
            {
                m_selectedPlanet = value;
                RemovePlanet.FireCanExecute();    //vola metodu CommandBase
                EditPlanet.FireCanExecute();
                FirePlanetChanged(m_selectedPlanet);
                EditProperties.FireCanExecute();   // Umozni stisk tlacitka
            }
        }


        //METHODS
        public bool CheckNullPlanet()
        {
            if (SelectedPlanet != null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// v teto metode bude kod ktery nastane kdyz se zmeni vlastnost na ViewModelu napr vyberem galaxie se zmeni vlastnost 
        /// mame instanci galaxyViewModelu a tedy i promenne.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="galaxy"></param>
        private void OnGalaxyChanged(object sender, GalaxyDataContract galaxy)
        {
            m_selectedGalaxy = galaxy;   //do promenne si ulozime vybranou galaxii
            AddPlanet.FireCanExecute();  //zavola delegata (=> m_selectedGalaxy)


            if (galaxy.Planets == null)
            {
                galaxy.Planets = m_planetDao.LoadPlanetsBasedOnGalaxyId(m_selectedGalaxy.Id).Select(x => PlanetDataContract.Create(x)).ToList();
            }

            ListOfPlanetsFromSelectedGalaxies = new ObservableCollection<PlanetDataContract>(galaxy.Planets);
        }


        /// <summary>
        /// Volano pomoci CommandBase AddPlanet , nabindovano v xaml jako Command= {Binding AddPlanet}.
        /// Vyvolano po kliknuti na tlacitko Pridat Planetu. 
        /// Otevre dialogove okno PlanetaDialogWindow.
        /// Na promenne viewModel pozna jake bylo stlaceno tlacitko.
        /// Na promenne viewModel bude vlastnost Planeta, kterou ulozi do kolekce ListOfPlanetsFromSelectedGalaxies.
        /// </summary>
        private void DoAddPlanet()
        {
            PlanetaDialogWindow pdw = new PlanetaDialogWindow();
            PlanetsDialogViewModel viewModel = new PlanetsDialogViewModel(m_selectedGalaxy.Id, pdw);
            pdw.DataContext = viewModel;

            pdw.ShowDialog();


            Console.WriteLine("Metoda DoAddPlanet");


            if (viewModel.OnSavePressed == true)
            {
                Console.WriteLine("viewModel.OnSavePressed == true");
                //TADY UDELAT REFRESH TABULKY PLANET V DB SE ZMENI ALE VE VIEW MA ID NULA  A KLIKNUTI NA NOVOU PLANETU HAZE VYJIMKU.

                m_planetDao.Insert(viewModel.Planeta.ConvertToDbEntity());                   // a muzeme ji upravit v db.

                //viewModel.Planeta.Id = id;
                m_selectedGalaxy.Planets.Add(viewModel.Planeta);

                ListOfPlanetsFromSelectedGalaxies.Add(viewModel.Planeta);
            }
        }


        /// <summary>
        /// Volano pomoci CommandBase RemovePlanet , nabindovano v xaml jako Command
        /// </summary>
        private void DoRemovePlanet()
        {
            Console.WriteLine("PlanetsViewModel.DoRemovePlanet");
            // Planete musime nejdrive odebrat vlastnostiPlanet. planeta.Properties


            int id = SelectedPlanet.Id;
            m_vlastnostiPlanetDao.RemoveAllVlastnostiPlanet(id);

            // muzeme odebrat planetu.
            m_planetDao.Delete(SelectedPlanet.ConvertToDbEntity());

            m_selectedGalaxy.Planets.Remove(SelectedPlanet);
            ListOfPlanetsFromSelectedGalaxies.Remove(SelectedPlanet);
        }


        /// <summary>
        /// Volano pomoci CommandBaseInstance EditPlanet, nabindovano v xaml jako Command {Binding EditPlanet}
        /// </summary>
        private void DoEditPlanet()
        {
            Console.WriteLine("doEditPlanet");
            //  dialog dostane viewModel , ten i po stisku tlacitka bude vedet ktere bylo zmacknuto 
            //  viewModel ovlada view a muze ho zavrit
            PlanetaDialogWindow pdw = new PlanetaDialogWindow();

            PlanetsDialogViewModel viewModel = new PlanetsDialogViewModel(SelectedPlanet, pdw);
            pdw.DataContext = viewModel;
            pdw.ShowDialog();

            if (viewModel.OnSavePressed == true)
            {
                Console.WriteLine("viewModel.OnSavePressed == true");
                m_planetDao.Update(viewModel.Planeta.ConvertToDbEntity());                   // a muzeme ji upravit v db.
            }
        }


        /// <summary>
        /// Volano pomoci CommandBaseInstance EditProperties, xaml command {Binding EditProperties}
        /// </summary>
        public void DoEditProperties()
        {
            EditPropertyDialogWindow adw = new EditPropertyDialogWindow();
            EditPropertyViewModel apvm = new EditPropertyViewModel(m_daoSource, adw, m_eventRegistrator, this);
            FirePlanetChanged(SelectedPlanet);
            adw.DataContext = apvm;

            adw.ShowDialog();
        }



        //EVENTY
        /// <summary>
        /// Zmena vybrane planety
        /// </summary>
        public event EventHandler<PlanetDataContract> OnPlanetChanged;
        public void FirePlanetChanged(PlanetDataContract planeta)
        {
            Console.WriteLine("PlanetsViewModel.FirePlanetChanged   , zmenila se vybrana planeta   MainWindow.SelectedPlanet");
            OnPlanetChanged?.Invoke(this, planeta);
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        #endregion

    }
}
