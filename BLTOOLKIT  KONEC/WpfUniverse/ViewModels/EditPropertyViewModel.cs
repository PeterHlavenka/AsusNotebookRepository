using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUniverse.Core;
using WpfUniverse.Entities;



namespace WpfUniverse.ViewModels
{
    class EditPropertyViewModel : INotifyPropertyChanged

    {

        private PlanetDataContract m_selectedPlanet;
        private IDialogWindow m_dialogWindow;
        private List<Vlastnost> m_VlastnostsOnPlanet; //entita
        private ObservableCollection<VlastnostDataContract> m_listOfAllPossibleVlastnosts;
        private IVlastnostDao m_vlastnostDao;
        private VlastnostDataContract m_selectedProperty;
        private string m_nameOfNewProperty;
        private int m_propertiesCount;
        private IPlanetDao m_planetDao;
        private IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        private IEventRegistrator m_IEventRegistrator;
        private IPlanetSelector m_IPlanetSelector;
        private ITransactionManager m_transactionManager;

        public EditPropertyViewModel(IDaoSource daoSource , IDialogWindow dialogWindow, IEventRegistrator registrator, IPlanetSelector selector)
        {
            m_IEventRegistrator = registrator;         // z parametru konstruktoru , potrebujeme v metode DoAllChanges , abychom videli do tridy PlanetViewModel a mohli odregistrovat jeji event.
            m_dialogWindow = dialogWindow;
            m_IPlanetSelector = selector;
            m_IPlanetSelector.OnPlanetChanged += OnPlanetChanged;
            m_vlastnostDao = daoSource.GetDaoByEntityType<IVlastnostDao,Vlastnost,int>();
            m_planetDao = daoSource.GetDaoByEntityType<IPlanetDao, Planeta, int>();
            m_vlastnostiPlanetDao = daoSource.GetMultiKeyDaoByEntityType<IVlastnostiPlanetDao,VlastnostiPlanet>();
            m_transactionManager = daoSource.TransactionManager;


            m_listOfAllPossibleVlastnosts = MainWindow.Manager.ListOfAllPossibleVlastnosts;

            SaveChanges = new CommandBase(() => true, DoSaveChanges);
            RemoveSelected = new CommandBase(() => SelectedProperty != null, DoRemoveSelected);
            CheckAll = new CommandBase(() => true, DoCheckAll);
            UncheckAll = new CommandBase(() => true, DoUncheckAll);
            AddNewProperty = new CommandBase(() => true, DoAddNewProperty);

            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }





        //COMMANDY
        public CommandBase SaveChanges { get; private set; }
        public CommandBase RemoveSelected { get; private set; }
        public CommandBase CheckAll { get; private set; }
        public CommandBase UncheckAll { get; private set; }
        public CommandBase AddNewProperty { get; private set; }



        //VLASTNOSTI
        public PlanetDataContract SelectedPlanet
        {
            get { return m_selectedPlanet; }
            set { m_selectedPlanet = value; }

        }
        public List<Vlastnost> VlastnostsOnPlanet
        {
            get { return m_VlastnostsOnPlanet; }
            set { m_VlastnostsOnPlanet = value; OnPropertyChanged(nameof(VlastnostsOnPlanet)); }
        }
        public ObservableCollection<VlastnostDataContract> ListOfAllPossibleVlastnosts
        {
            get { return m_listOfAllPossibleVlastnosts; }
            set { m_listOfAllPossibleVlastnosts = value; OnPropertyChanged(nameof(ListOfAllPossibleVlastnosts)); }
        }
        public VlastnostDataContract SelectedProperty
        {
            get { return m_selectedProperty; }
            set { m_selectedProperty = value; OnPropertyChanged(nameof(SelectedProperty)); RemoveSelected.FireCanExecute(); }
        }
        public string NameOfNewProperty
        {
            get { return m_nameOfNewProperty; }
            set { m_nameOfNewProperty = value; OnPropertyChanged(nameof(NameOfNewProperty)); Console.WriteLine(m_nameOfNewProperty); }
        }
        public int PropertiesCount
        {
            get { return m_propertiesCount; }
            set { m_propertiesCount = value; OnPropertyChanged(nameof(PropertiesCount)); }
        }



        //METODY



        private void DoSaveChanges()  // Ok
        {
            foreach (var vdc in ListOfAllPossibleVlastnosts)
            {
                Vlastnost vlastnost = vdc.ConvertToDbEntity();
                m_vlastnostDao.Update(vlastnost);                          // Tady funguje zavolani rovnou Update na dao tride
            }
        }

        private void DoRemoveSelected()       // ok
        {


            var query = from vdc in ListOfAllPossibleVlastnosts
                        where vdc.Id == SelectedProperty.Id
                        select vdc;

            Vlastnost vlastnost = query.First().ConvertToDbEntity();

            m_vlastnostDao.DeleteVlastnost(vlastnost);                             //odebere z databaze     

            ListOfAllPossibleVlastnosts.Remove(query.First());


            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }

        private void DoAllChanges(bool value)
        {
            Action action = () =>
            {
                try
                {
                    m_IEventRegistrator.UnregisterFromEvent();     // Odregistrujeme event tridy PlanetViewModel 

                    foreach (VlastnostDataContract vdc in ListOfAllPossibleVlastnosts)
                    {
                        if (vdc.IsChecked != value)
                        {
                            vdc.IsChecked = value;
                            if (value)
                            {
                                SelectedPlanet.Properties.Add(new Vlastnost() { Id = vdc.Id, Nazev = vdc.Nazev });
                                m_vlastnostiPlanetDao.InsertPropertyToPlanet(new VlastnostiPlanet { PlanetaId = SelectedPlanet.Id, VlastnostId = vdc.Id });
                            }
                            else
                            {
                                SelectedPlanet.Properties.RemoveAll(d => d.Id == vdc.Id);
                                m_vlastnostiPlanetDao.RemovePropertyFromPlanet(SelectedPlanet.Id, vdc.Id);
                            }
                        }
                    }
                }
                finally
                {
                    m_IEventRegistrator.RegisterToEvent();
                }
            };
            
            m_transactionManager.CallInsideTransaction(action, m_vlastnostiPlanetDao.CreateCommonDbContext());
        }

        private void DoCheckAll()  //        
        {


            //Zmenime vsechny hodnoty isChecked na true
            DoAllChanges(true);
        }

        private void DoUncheckAll()       //  ok
        {
            //Zmenime vsechny hodnoty isChecked na FALSE    
            DoAllChanges(false);

        }

        private void DoAddNewProperty()       // ok  
        {
            Vlastnost vlastnost = new Vlastnost();
            vlastnost.Nazev = NameOfNewProperty;

            m_vlastnostDao.Insert(vlastnost);

            ListOfAllPossibleVlastnosts.Add(VlastnostDataContract.Create(vlastnost));

            PropertiesCount = ListOfAllPossibleVlastnosts.Count;
        }

       

        /// <summary>
        /// Zmena vybrane planety. Tato metoda zachytava event vyhozeny na tride PlanetViewModel kdyz se zmeni vyber planety.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="selectedPlanet"></param>
        private void OnPlanetChanged(object sender, PlanetDataContract selectedPlanet)
        {
            // Odpojime posluchace eventu ktery vyhazuje VlastnostDataContract kdyz se mu zmeni nejaka vlastnost.
            // Deje se tak proto, aby jsme si uvodni kontrolou nevyhazovali event na datacontractu.  Budeme totiz menit vlastnosti isChecked.
            m_IEventRegistrator.UnregisterFromEvent();

            SelectedPlanet = selectedPlanet;                              // Do vlastnosti vlozime hodnotu z parametru.
         
            // Pripojime posluchace eventu ktery vyhazuje VlastnostDataContract kdyz se mu zmeni nejaka vlastnost.
            if (SelectedPlanet != null)
            {
                m_IEventRegistrator.RegisterToEvent();
            }
        }

        

        #region INotifyProperty Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        #endregion


    }
}
