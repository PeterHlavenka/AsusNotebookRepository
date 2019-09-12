using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.Gui;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Requests;
using WpfUniverse.Gui.Interfaces;

namespace WpfUniverse.Gui.ViewModels
{
    public class GalaxyViewModel : ViewModelBase, IGalaxySelector
    {
        private readonly GalaxyDialogViewModel m_galaxyDialogViewModel;
        private GalaxyDataContract m_selectedGalaxy;
        private List<GalaxyDataContract> m_selectedGalaxies = new List<GalaxyDataContract>();
        public readonly List<GalaxyDataContract> ListOfThree = new List<GalaxyDataContract>();
        private readonly IWindowManager m_windowManager;
        private readonly IClientToServicePublisher m_clientToServicePublisher;


        public GalaxyViewModel(GalaxyDialogViewModel m_galaxyDialogViewModel, IWindowManager windowManager, IClientToServicePublisher clientToServicePublisher)  // tady budou zavislosti jen na veci ktere umi castle resolvovat 
        {
            this.m_galaxyDialogViewModel = m_galaxyDialogViewModel;
            m_windowManager = windowManager;
            m_clientToServicePublisher = clientToServicePublisher;           
            EditGalaxy = new RelayCommand(DoEditGalaxy , () => SelectedGalaxy != null );
            SelectThree = new RelayCommand(DoSelectThree, () => true);
        }

        public void Initialize()
        {
            var response = m_clientToServicePublisher.Publish(new SelectAllGalaxiesRequest());

            ListOfGalaxies = new BindableCollection<GalaxyDataContract>(response.Galaxies.ToList());   
        }

        private void DoSelectThree()
        {
            ListOfThree.Clear();

            for (int i = 0; i < 3; i++)
            {
                ListOfThree.Add(ListOfGalaxies[i]);
            }
          
            SelectedGalaxies = ListOfThree;
        }

        public ICommand EditGalaxy { get; }
        public ICommand SelectThree { get; }
        public BindableCollection<GalaxyDataContract> ListOfGalaxies { get; set; }



        public IList SelectedGalaxies
        {
            get { return m_selectedGalaxies; }
            set
            {
                m_selectedGalaxies = value?.Cast<GalaxyDataContract>().ToList();                              
                NotifyOfPropertyChange(nameof(SelectedGalaxies));
                FireGalaxiesChanged(m_selectedGalaxies);
            }
        }

        public GalaxyDataContract SelectedGalaxy
        {
            get => m_selectedGalaxy;
            set
            {
                m_selectedGalaxy = value;
               NotifyOfPropertyChange(nameof(SelectedGalaxy));             
            }
        }


        public event EventHandler<List<GalaxyDataContract>> OnGalaxiesChanged;

        public void FireGalaxiesChanged(List<GalaxyDataContract> galaxies)
        {
            OnGalaxiesChanged?.Invoke(this, galaxies);
        }

        private void DoEditGalaxy()                    // tady zavolame jen inicializacni metodu a po ni windowManager.show() o zbytek se postara caliburn
        {
            m_galaxyDialogViewModel.Initialize(SelectedGalaxy);

            if (m_windowManager.ShowDialog(m_galaxyDialogViewModel) == true) // pokud metoda tryclose vrati true
            {
                var response = m_clientToServicePublisher.Publish(new UpdateGalaxyRequest(SelectedGalaxy));
                ListOfGalaxies.Remove(SelectedGalaxy);
                SelectedGalaxy = response.Galaxy;
                ListOfGalaxies.Add(SelectedGalaxy);
               
            }
        }



        public event EventHandler<GalaxyDataContract> OnGalaxyChanged;

        public void FireGalaxyChanged(GalaxyDataContract galaxy)
        {
            OnGalaxyChanged?.Invoke(this, galaxy);
        }
    }
}