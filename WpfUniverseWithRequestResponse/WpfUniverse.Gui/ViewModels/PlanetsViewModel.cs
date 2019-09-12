using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.Gui;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Requests;
using WpfUniverse.Gui.Commands;
using WpfUniverse.Gui.Interfaces;
using WpfUniverse.Gui.Wrappers;

namespace WpfUniverse.Gui.ViewModels
{
    public class PlanetsViewModel : ViewModelBase         //, IPlanetSelector
    {
        private readonly IClientToServicePublisher m_clientToServicePublisher;
        private readonly PropertiesViewModel m_propertiesViewModel;

        private readonly EditPropertyViewModel m_editPropertyViewModel;
        //private IEventRegistrator m_eventRegistrator;
        private BindableCollection<PlanetDataContract> m_listOfPlanetsFromSelectedGalaxies;
        //private readonly IPlanetDao m_planetDao;
        private readonly PlanetsDialogViewModel m_planetsDialogViewModel;
        private GalaxyDataContract m_selectedGalaxy;
        private PlanetDataContract m_selectedPlanet;
        //private readonly ITransactionManager m_transactionManager;
        //private readonly IVlastnostDao m_vlastnostDao;
        //private readonly IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        //private readonly IPropertiesManager m_vlastnostManager;
        private readonly IWindowManager m_windowManager;



        //public void Initialize()
        //{
            
        //}

        public PlanetsViewModel(
            IGalaxySelector selector,
            PlanetsDialogViewModel planetsDialogViewModel, IWindowManager windowManager,
            EditPropertyViewModel editPropertyViewModel, IClientToServicePublisher clientToServicePublisher, PropertiesViewModel propertiesViewModel)
        {
            //m_planetDao = planetDao;
            //m_vlastnostiPlanetDao = vlastnostiPlanetDao;
            selector.OnGalaxiesChanged += OnGalaxiesChanged;
            //m_transactionManager = daoSource.TransactionManager;
            //m_vlastnostManager = vlastnostManager;
            //m_vlastnostDao = vlastnostDao;
            m_planetsDialogViewModel = planetsDialogViewModel;
            m_windowManager = windowManager;
            m_editPropertyViewModel = editPropertyViewModel;
            m_windowManager = windowManager;
            m_clientToServicePublisher = clientToServicePublisher;
            m_propertiesViewModel = propertiesViewModel;

            ListOfPlanetsFromSelectedGalaxies = new BindableCollection<PlanetDataContract>();

            AddPlanet = new RelayCommand(DoAddPlanet, () => m_selectedGalaxy != null);
            RemovePlanet = new RelayCommand(DoRemovePlanet, CheckNullPlanet);
            EditPlanet = new RelayCommand(DoEditPlanet, CheckNullPlanet);
            EditProperties = new RelayCommand(DoEditProperties , CheckNullPlanet);
            SaveChangesCommand = new RelayCommand(SaveChanges);

            //Initialize();
        }

        public ICommand AddPlanet { get; }
        public ICommand RemovePlanet { get; }
        public ICommand EditPlanet { get; }
        public ICommand EditProperties { get; }

        public ICommand SaveChangesCommand { get; }

        public PropertiesViewModel PropertiesViewModel { get; set; }

        public BindableCollection<PlanetDataContract> ListOfPlanetsFromSelectedGalaxies
        {
            get => m_listOfPlanetsFromSelectedGalaxies;
            set
            {
                m_listOfPlanetsFromSelectedGalaxies = value;
                NotifyOfPropertyChange(nameof(ListOfPlanetsFromSelectedGalaxies));

            }
        }

        public PlanetDataContract SelectedPlanet
        {
            get => m_selectedPlanet;
            set
            {
                m_selectedPlanet = value;
                if (m_selectedPlanet == null)
                {
                    m_propertiesViewModel.Initialize(0);
                }
                else
                {
                    m_propertiesViewModel.Initialize(m_selectedPlanet.Id);
                }
                

            }
        }


        //public void SetPropertiesDependency(IEventRegistrator registrator)
        //{
        //    m_eventRegistrator = registrator;
        //}


        public void FirePlanetChanged(PlanetDataContract planeta)
        {
            //PropertiesViewModel.
        }


        public bool CheckNullPlanet()
        {
            return SelectedPlanet != null;
        }


        private void OnGalaxiesChanged(object sender, List<GalaxyDataContract> galaxies)
        {
            ListOfPlanetsFromSelectedGalaxies.Clear();

            if (galaxies.Count == 1)
                m_selectedGalaxy = galaxies[0];
            else
                m_selectedGalaxy = null;


            foreach (var g in galaxies)
            {
                var response = m_clientToServicePublisher.Publish(new SelectPlanetsRequest(g.Id));
                g.Planets = response.Planets;

                ListOfPlanetsFromSelectedGalaxies.AddRange(response.Planets);
            }
        }


        private void DoAddPlanet()
        {           
            var pdc = new PlanetDataContract(0, "", 0, m_selectedGalaxy.Id, Guid.NewGuid());

            m_planetsDialogViewModel.Initialize(pdc);

            if (m_windowManager.ShowDialog(m_planetsDialogViewModel) == true)
            {
                var response = m_clientToServicePublisher.Publish(new InsertPlanetRequest(pdc));

                ListOfPlanetsFromSelectedGalaxies.Add(response.PlanetDataContract);  
            }
            ;
        }


        private void DoRemovePlanet()
        {

            var res = m_clientToServicePublisher.Publish(new DeleteVlastnostsFromPlanetRequest(SelectedPlanet.Id));
            var response = m_clientToServicePublisher.Publish(new DeletePlanetRequest(SelectedPlanet.Id));
            ListOfPlanetsFromSelectedGalaxies.Remove(SelectedPlanet);
            m_selectedGalaxy.Planets.Remove(SelectedPlanet);


        }


        private void DoEditPlanet()
        {
            m_planetsDialogViewModel.Initialize(SelectedPlanet);
            if (m_windowManager.ShowDialog(m_planetsDialogViewModel) == true)
            {

                var response = m_clientToServicePublisher.Publish(new UpdatePlanetRequest(SelectedPlanet));
                ListOfPlanetsFromSelectedGalaxies.Remove(SelectedPlanet);
                SelectedPlanet = response.Planet;
                ListOfPlanetsFromSelectedGalaxies.Add(SelectedPlanet);
            }
        }

        private void DoEditProperties()
        {
            m_editPropertyViewModel.Initialize(m_propertiesViewModel.ListOfVlastnosts, m_clientToServicePublisher, SelectedPlanet);

            m_windowManager.ShowDialog(m_editPropertyViewModel);

            //if (m_editPropertyViewModel.IsDirty)
            //{
            //    m_propertiesViewModel.PropertiesChanged()
            //}
        }

        private void SaveChanges()
        {


            List<VlastnostWrapper> wrapperList = m_propertiesViewModel.ListOfVlastnosts.ToList();
            List<VlastnostDataContract> vdcList = new List<VlastnostDataContract>();

            foreach (var VARIABLE in wrapperList)
            {
                VlastnostDataContract vdc = new VlastnostDataContract();
                vdc.Id = VARIABLE.Id;   // toto je zaroven id vlastnosti
                vdc.Nazev = VARIABLE.Nazev;
                if (VARIABLE.IsChecked)
                {
                    vdcList.Add(vdc);            // jen checknute
                }

            }

            var response = m_clientToServicePublisher.Publish(new SaveChangesRequest(vdcList, SelectedPlanet.Id));
        }
    }
}