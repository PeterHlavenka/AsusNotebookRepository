using System;
using System.Collections.ObjectModel;
using System.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.Views;

namespace WpfUniverse.ViewModels
{
    public class PlanetsViewModel : ViewModelBase, IPlanetSelector
    {
        private readonly IPlanetDao m_planetDao;
        private readonly ITransactionManager m_transactionManager;
        private readonly IVlastnostDao m_vlastnostDao;
        private readonly IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        private readonly IPropertiesManager m_vlastnostManager;
        private IEventRegistrator m_eventRegistrator;
        private ObservableCollection<PlanetDataContract> m_listOfPlanetsFromSelectedGalaxies;
        private GalaxyDataContract m_selectedGalaxy;
        private PlanetDataContract m_selectedPlanet;

        public PlanetsViewModel(IDaoSource daoSource, IPlanetDao planetDao, IVlastnostDao vlastnostDao,
            IVlastnostiPlanetDao vlastnostiPlanetDao, IGalaxySelector selector, IPropertiesManager vlastnostManager)
        {
            m_planetDao = planetDao;
            m_vlastnostiPlanetDao = vlastnostiPlanetDao;
            selector.OnGalaxyChanged += OnGalaxyChanged;
            m_transactionManager = daoSource.TransactionManager;
            m_vlastnostManager = vlastnostManager;
            m_vlastnostDao = vlastnostDao;

            ListOfPlanetsFromSelectedGalaxies = new ObservableCollection<PlanetDataContract>();

            AddPlanet = new CommandBase(() => m_selectedGalaxy != null, DoAddPlanet);
            RemovePlanet = new CommandBase(CheckNullPlanet, DoRemovePlanet);
            EditPlanet = new CommandBase(CheckNullPlanet, DoEditPlanet);
            EditProperties = new CommandBase(CheckNullPlanet, DoEditProperties);
        }


        public CommandBase AddPlanet { get; }
        public CommandBase RemovePlanet { get; }
        public CommandBase EditPlanet { get; }
        public CommandBase EditProperties { get; }


        public ObservableCollection<PlanetDataContract> ListOfPlanetsFromSelectedGalaxies
        {
            get => m_listOfPlanetsFromSelectedGalaxies;
            set
            {
                m_listOfPlanetsFromSelectedGalaxies = value;
                OnPropertyChanged(nameof(ListOfPlanetsFromSelectedGalaxies));
            }
        }

        public PlanetDataContract SelectedPlanet
        {
            get => m_selectedPlanet;
            set
            {
                m_selectedPlanet = value;
                RemovePlanet.FireCanExecute();
                EditPlanet.FireCanExecute();
                FirePlanetChanged(m_selectedPlanet);
                EditProperties.FireCanExecute();
            }
        }


        public void SetPropertiesDependency(IEventRegistrator registrator)
        {
            m_eventRegistrator = registrator;
        }

        public event EventHandler<PlanetDataContract> OnPlanetChanged;

        public void FirePlanetChanged(PlanetDataContract planeta)
        {
            OnPlanetChanged?.Invoke(this, planeta);
        }


        public bool CheckNullPlanet()
        {
            return SelectedPlanet != null;
        }


        private void OnGalaxyChanged(object sender, GalaxyDataContract galaxy)
        {
            m_selectedGalaxy = galaxy;
            AddPlanet.FireCanExecute();


            if (galaxy.Planets == null)
                galaxy.Planets = m_planetDao.LoadPlanetsBasedOnGalaxyId(m_selectedGalaxy.Id)
                    .Select(PlanetDataContract.Create).ToList();

            ListOfPlanetsFromSelectedGalaxies = new ObservableCollection<PlanetDataContract>(galaxy.Planets);
        }


        private void DoAddPlanet()
        {
            var pdw = new PlanetDialogWindow();
            var viewModel = new PlanetsDialogViewModel(m_selectedGalaxy.Id, pdw);
            pdw.DataContext = viewModel;

            pdw.ShowDialog();

            if (!viewModel.OnSavePressed) return;

            m_planetDao.Insert(viewModel.Planeta.ConvertToDbEntity());
            m_selectedGalaxy.Planets.Add(viewModel.Planeta);
            ListOfPlanetsFromSelectedGalaxies.Add(viewModel.Planeta);
        }


        private void DoRemovePlanet()
        {
            var id = SelectedPlanet.Id;
            m_vlastnostiPlanetDao.RemoveAllVlastnostiPlanet(id);

            m_planetDao.Delete(SelectedPlanet.ConvertToDbEntity());

            m_selectedGalaxy.Planets.Remove(SelectedPlanet);
            ListOfPlanetsFromSelectedGalaxies.Remove(SelectedPlanet);
        }


        private void DoEditPlanet()
        {
            var pdw = new PlanetDialogWindow();

            var viewModel = new PlanetsDialogViewModel(SelectedPlanet, pdw);
            pdw.DataContext = viewModel;
            pdw.ShowDialog();

            if (viewModel.OnSavePressed)
                m_planetDao.Update(viewModel.Planeta.ConvertToDbEntity());
        }

        public void DoEditProperties()
        {
            var adw = new EditPropertyDialogWindow();
            var apvm = new EditPropertyViewModel(m_vlastnostDao, m_vlastnostiPlanetDao, m_transactionManager,
                m_eventRegistrator, this, m_vlastnostManager);
            FirePlanetChanged(SelectedPlanet);
            adw.DataContext = apvm;

            adw.ShowDialog();
        }
    }
}