using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Navigation;
using Caliburn.Micro;
using FluentValidation;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.Gui;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Requests;
using WpfUniverse.Gui.Commands;
using WpfUniverse.Gui.Errors;
using WpfUniverse.Gui.Wrappers;

namespace WpfUniverse.Gui.ViewModels
{
    public class EditPropertyViewModel : ErrorBase

    {
       
        private string m_nameOfNewProperty;


        public EditPropertyViewModel()
        {         
            SaveNameChanges = new CommandBase(() => true, DoSaveNameChanges);
            RemoveSelected = new CommandBase(() => SelectedProperty != null, DoRemoveSelected);
            CheckAll = new CommandBase(() => true, DoCheckAll);
            UncheckAll = new CommandBase(() => true, DoUncheckAll);
            AddNewProperty = new RelayCommand(DoAddNewProperty, () => IsValid);

            Validator = new CustomValidator();
        }


        public CommandBase SaveNameChanges { get; }
        public CommandBase RemoveSelected { get; }
        public CommandBase CheckAll { get; }
        public CommandBase UncheckAll { get; }
        public ICommand AddNewProperty { get; }


        private PlanetDataContract m_selectedPlanet;

        public PlanetDataContract SelectedPlanet
        {
            get => m_selectedPlanet;
        }

        private VlastnostWrapper SelectedProperty { get; set; }


        private IClientToServicePublisher m_clientToServicePublischer;
        public void Initialize(IEnumerable<VlastnostWrapper> properties , IClientToServicePublisher clientToServicePublisher,  PlanetDataContract selectedPlanet)
        {
            ListOfAllPossibleVlastnosts.Clear();
            ListOfAllPossibleVlastnosts.AddRange(properties);
            m_clientToServicePublischer = clientToServicePublisher;
            m_selectedPlanet = selectedPlanet;
           
        }

        

        private BindableCollection<VlastnostWrapper> m_listOfAllPossibleVlastnosts = new BindableCollection<VlastnostWrapper>();
        

        public BindableCollection<VlastnostWrapper> ListOfAllPossibleVlastnosts
        {
            get => m_listOfAllPossibleVlastnosts;
            set
            {
                m_listOfAllPossibleVlastnosts = value;
                NotifyOfPropertyChange(nameof(ListOfAllPossibleVlastnosts));               
            }
        }

        

        public string NameOfNewProperty
        {
            get => m_nameOfNewProperty;
            set
            {
                m_nameOfNewProperty = value;
               
                NotifyOfPropertyChange(nameof(NameOfNewProperty));
                NotifyOfPropertyChange(nameof(Error));
                NotifyOfPropertyChange(nameof(IsValid));               
            }
        }


        private void DoSaveNameChanges()
        {
            m_clientToServicePublischer.Publish(new UpdateNameOfPropertyRequest(NameOfNewProperty ,  SelectedProperty.Id));
            //IsDirty = true;        // ZNAMENA ZE SE NAM NEJAKA KOLEKCE ZMENILA
        }

        private void DoRemoveSelected()
        {
            m_clientToServicePublischer.Publish((new RemoveSelectedPropertyRequest(SelectedPlanet.Id, SelectedProperty.Id)));
        }





        private void DoCheckAll()
        {
            m_clientToServicePublischer.Publish(new CheckAllPropertiesRequest(SelectedPlanet.Id));

            foreach (VlastnostWrapper wrapper in ListOfAllPossibleVlastnosts)
            {
                wrapper.IsChecked = true;
            }
        }

        private void DoUncheckAll()
        {
            m_clientToServicePublischer.Publish(new UncheckAllPropertiesRequest(SelectedPlanet.Id));       

            foreach (VlastnostWrapper wrapper in ListOfAllPossibleVlastnosts)
            {
                wrapper.IsChecked = false;
            }
        }

        private void DoAddNewProperty()
        {
          var response =  m_clientToServicePublischer.Publish(new AddPropertyRequest(NameOfNewProperty));
            // add to list
            ListOfAllPossibleVlastnosts.Add(new VlastnostWrapper(response.Contract));
            
        }



        private class CustomValidator : AbstractValidator<EditPropertyViewModel>
        {
            public CustomValidator()
            {
                RuleFor(d => d.NameOfNewProperty).Length(0, 4)
                    .WithMessage("Nazev nove vlastnosti musi mit max 4 znaky");
            }
        }
    }
}