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
    public class PlanetsViewModel : INotifyPropertyChanged
    {
        private PlanetDao m_planetDao;
        private GalaxyViewModel m_galaxyViewModel;
        private ObservableCollection<PlanetDataContract> m_ListOfPlanetsFromSelectedGalaxies;             //PRETYPOVANO Z OBSERVABLECOLLECTION
        private GalaxyDataContract m_selectedGalaxy;
        private PlanetDataContract m_selectedPlanet;
        private VlastnostiPlanetDao m_vlastnostiPlanetDao;

        public PlanetsViewModel(GalaxyViewModel galaxyViewModel)
        {
            m_planetDao = new PlanetDao(MainWindow.Conn); //nacte planety 
            m_vlastnostiPlanetDao = new VlastnostiPlanetDao(MainWindow.Conn);
            
            ListOfPlanetsFromSelectedGalaxies = new ObservableCollection<PlanetDataContract>();

           

            m_galaxyViewModel = galaxyViewModel;  //pridal jsem privatni atribut abych se dostal k vlastnostem GalaxyVM
            m_galaxyViewModel.OnGalaxyChanged += OnGalaxyChanged;  //registrace posluchace

            AddPlanet = new CommandBase(() => m_selectedGalaxy != null, DoAddPlanet);
            RemovePlanet = new CommandBase(CheckNullPlanet, DoRemovePlanet);
            EditPlanet = new CommandBase(CheckNullPlanet, DoEditPlanet);
        }

//COMMANDY
        public CommandBase AddPlanet { get; private set; }
        public CommandBase RemovePlanet { get; private set; }
        public CommandBase EditPlanet { get; private set; }


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
                OnPropertyChanged(nameof(SelectedPlanet));
                RemovePlanet.FireCanExecute();    //vola metodu CommandBase
                EditPlanet.FireCanExecute();
                FirePlanetChanged(m_selectedPlanet);  //
            }
        }


       



//METODY
        public bool CheckNullPlanet()
        {
            if (m_selectedPlanet != null)
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

                int id = m_planetDao.InsertPlanet(viewModel.Planeta.ConvertToDbEntity());                   // a muzeme ji upravit v db.
                
                viewModel.Planeta.Id = id;
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
            m_vlastnostiPlanetDao.RemovAllVlastnostiPlanet(SelectedPlanet.Id);

            // muzeme odebrat planetu.
            m_planetDao.RemovePlanet(SelectedPlanet.Id);   

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
                m_planetDao.UpdatePlanet(viewModel.Planeta.ConvertToDbEntity());                   // a muzeme ji upravit v db.
            }
        }



//EVENTY
        public event EventHandler<PlanetDataContract> OnPlanetChanged;
        public void FirePlanetChanged(PlanetDataContract planeta)
        {
            Console.WriteLine("PlanetsViewModel.FirePlanetChanged");
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
