using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.Utilities.Extensions;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Requests;
using WpfUniverse.Gui.Wrappers;

namespace WpfUniverse.Gui.ViewModels
{
    public class PropertiesViewModel : ViewModelBase //, IEventRegistrator
    {
        private readonly IList<VlastnostWrapper> m_allProperties;
        private readonly IClientToServicePublisher m_serverProxy;

        public PropertiesViewModel(IClientToServicePublisher serverProxy)
        {
            m_serverProxy = serverProxy;
            m_allProperties = serverProxy.Publish(new SelectPropertiesRequest()).Items.Select(d=> new VlastnostWrapper(d)).ToList();
        }


        private BindableCollection<VlastnostWrapper> m_listOfVlastnost = new BindableCollection<VlastnostWrapper>();
        public BindableCollection<VlastnostWrapper> ListOfVlastnosts
        {
            get => m_listOfVlastnost;
            set
            {
                m_listOfVlastnost = value;
                NotifyOfPropertyChange(nameof(ListOfVlastnosts));
               
            }
        }

        private PlanetDataContract m_selectedPlanet;
        public PlanetDataContract SelectedPlanet
        {
            get => m_selectedPlanet;
            set
            {
                m_selectedPlanet = value;
                NotifyOfPropertyChange(nameof(SelectedPlanet));
            }
        }

        public void Initialize(int planetId)
        {
            ListOfVlastnosts.Clear();


            var response = m_serverProxy.Publish(new GetCheckedRequest(planetId));

            List<VlastnostWrapper> items = new List<VlastnostWrapper>();
            items.AddRange(m_allProperties.ToList());

            items.ForEach(d => d.IsChecked = response.Vlastnosts.Any(x => x.Id == d.Id));
            ListOfVlastnosts.AddRange(items);

        }
    }
}