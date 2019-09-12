using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using WpfUniverse.Core;
using WpfUniverse.Entities;

namespace WpfUniverse.ViewModels
{
    public class EditPropertyViewModel : ViewModelBase

    {
        private readonly IEventRegistrator m_iEventRegistrator;
        private readonly ITransactionManager m_transactionManager;
        private readonly IVlastnostDao m_vlastnostDao;
        private readonly IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        private ObservableCollection<VlastnostDataContract> m_listOfAllPossibleVlastnosts;
        private string m_nameOfNewProperty;
        private int m_propertiesCount;
        private VlastnostDataContract m_selectedProperty;
        private List<Vlastnost> m_vlastnostsOnPlanet; //entita

        public EditPropertyViewModel(IVlastnostDao vlastnostDao, IVlastnostiPlanetDao vlastnostiPlanetDao,
            ITransactionManager transactionManager, IEventRegistrator registrator, IPlanetSelector selector,
            IPropertiesManager propertiesManager)
        {
            m_iEventRegistrator = registrator;

            selector.OnPlanetChanged += OnPlanetChanged;
            m_vlastnostDao = vlastnostDao;
            m_vlastnostiPlanetDao = vlastnostiPlanetDao;
            m_transactionManager = transactionManager;


            m_listOfAllPossibleVlastnosts = propertiesManager.ListOfAllPossibleVlastnosts;

            SaveChanges = new CommandBase(() => true, DoSaveChanges);
            RemoveSelected = new CommandBase(() => SelectedProperty != null, DoRemoveSelected);
            CheckAll = new CommandBase(() => true, DoCheckAll);
            UncheckAll = new CommandBase(() => true, DoUncheckAll);
            AddNewProperty = new CommandBase(() => true, DoAddNewProperty);

            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }


        public CommandBase SaveChanges { get; }
        public CommandBase RemoveSelected { get; }
        public CommandBase CheckAll { get; }
        public CommandBase UncheckAll { get; }
        public CommandBase AddNewProperty { get; }


        public PlanetDataContract SelectedPlanet { get; set; }

        public List<Vlastnost> VlastnostsOnPlanet
        {
            get => m_vlastnostsOnPlanet;
            set
            {
                m_vlastnostsOnPlanet = value;
                OnPropertyChanged(nameof(VlastnostsOnPlanet));
            }
        }

        public ObservableCollection<VlastnostDataContract> ListOfAllPossibleVlastnosts
        {
            get => m_listOfAllPossibleVlastnosts;
            set
            {
                m_listOfAllPossibleVlastnosts = value;
                OnPropertyChanged(nameof(ListOfAllPossibleVlastnosts));
            }
        }

        public VlastnostDataContract SelectedProperty
        {
            get => m_selectedProperty;
            set
            {
                m_selectedProperty = value;
                OnPropertyChanged(nameof(SelectedProperty));
                RemoveSelected.FireCanExecute();
            }
        }

        public string NameOfNewProperty
        {
            get => m_nameOfNewProperty;
            set
            {
                m_nameOfNewProperty = value;
                OnPropertyChanged(nameof(NameOfNewProperty));
                Console.WriteLine(m_nameOfNewProperty);
            }
        }

        public int PropertiesCount
        {
            get => m_propertiesCount;
            set
            {
                m_propertiesCount = value;
                OnPropertyChanged(nameof(PropertiesCount));
            }
        }


        private void DoSaveChanges()
        {
            foreach (var vdc in ListOfAllPossibleVlastnosts)
            {
                var vlastnost = vdc.ConvertToDbEntity();
                m_vlastnostDao.Update(vlastnost);
            }
        }

        private void DoRemoveSelected()
        {
            var query = from vdc in ListOfAllPossibleVlastnosts
                        where vdc.Id == SelectedProperty.Id
                        select vdc;

            var vlastnost = query.First().ConvertToDbEntity();

            m_vlastnostDao.Delete(vlastnost);

            ListOfAllPossibleVlastnosts.Remove(query.First());
            SelectedPlanet.Properties.Remove(vlastnost);
           

            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }

        public void DoRemoveSelected(int selectedId)
        {
            var query = from vdc in ListOfAllPossibleVlastnosts
                where vdc.Id == selectedId 
                select vdc;

            var vlastnost = query.First().ConvertToDbEntity();

            m_vlastnostDao.Delete(vlastnost);

            ListOfAllPossibleVlastnosts.Remove(query.First());
            SelectedPlanet.Properties.Remove(vlastnost);
           
            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }

        public void DoAllChanges(bool value)
        {
            void Action()
            {
                try
                {
                    m_iEventRegistrator.UnregisterFromEvent();

                    foreach (var vdc in ListOfAllPossibleVlastnosts)
                        if (vdc.IsChecked != value)
                        {
                            vdc.IsChecked = value;
                            if (value)
                            {
                                SelectedPlanet.Properties.Add(new Vlastnost { Id = vdc.Id, Nazev = vdc.Nazev });
                                m_vlastnostiPlanetDao.InsertPropertyToPlanet(new VlastnostiPlanet
                                {
                                    PlanetaId = SelectedPlanet.Id,
                                    VlastnostId = vdc.Id
                                });
                            }
                            else
                            {
                                SelectedPlanet.Properties.RemoveAll(d => d.Id == vdc.Id);
                                m_vlastnostiPlanetDao.RemovePropertyFromPlanet(SelectedPlanet.Id, vdc.Id);
                            }
                        }
                }
                finally
                {
                    m_iEventRegistrator.RegisterToEvent();
                }
            }

            m_transactionManager.CallInsideTransaction(Action, m_vlastnostiPlanetDao.CreateCommonDbContext(),
                IsolationLevel.Serializable);
        }

        private void DoCheckAll()
        {
            DoAllChanges(true);
        }

        private void DoUncheckAll()
        {
            DoAllChanges(false);
        }

        private void DoAddNewProperty()
        {
            var vlastnost = new Vlastnost();
            vlastnost.Nazev = NameOfNewProperty;

            m_vlastnostDao.Insert(vlastnost);

            ListOfAllPossibleVlastnosts.Add(VlastnostDataContract.Create(vlastnost));

            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }


        private void OnPlanetChanged(object sender, PlanetDataContract selectedPlanet)
        {
            m_iEventRegistrator.UnregisterFromEvent();

            SelectedPlanet = selectedPlanet;


            if (SelectedPlanet != null)
                m_iEventRegistrator.RegisterToEvent();
        }
    }
}