using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Core;
using WpfUniverse.Entities;



namespace WpfUniverse.ViewModels
{
    public class PropertiesViewModel : INotifyPropertyChanged, IEventRegistrator
    {
        private IPlanetSelector m_IPlanetSelector;
        private PlanetDataContract m_selectedPlanet;
        private ObservableCollection<VlastnostDataContract> m_listOfVlastnost;
        private VlastnostDataContract m_vlastnostDataContract;
        private IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        private IVlastnostDao m_vlastnostDao;
        private VlastnostsManager m_manager;

        //KONSTRUKTORY
        public PropertiesViewModel( IDaoSource daoSource , IPlanetSelector selector)
        { 
            m_manager = MainWindow.Manager;                                                  // Manager vlastnosti bude ten z MainWindow
            ListOfVlastnosts = MainWindow.Manager.ListOfAllPossibleVlastnosts;               // Seznam vlastnosti bude ten jeho seznam.

            m_IPlanetSelector = selector;
            selector.SetPropertiesDependency(this);

            m_vlastnostDataContract = new VlastnostDataContract();
            m_vlastnostiPlanetDao = daoSource.GetMultiKeyDaoByEntityType<IVlastnostiPlanetDao, VlastnostiPlanet>();
            m_vlastnostDao = daoSource.GetDaoByEntityType<IVlastnostDao, Vlastnost, int>();


            m_IPlanetSelector.OnPlanetChanged += OnPlanetChanged;                            // Zaregistrujeme posluchace pro zmenu planety.
        }

        

        //VLASTNOSTI
        /// <summary>
        /// Kolekce datacontractu pro binding . Je to kopie seznamu vlastnosti na manageru vlastnosti.
        /// </summary>
        public ObservableCollection<VlastnostDataContract> ListOfVlastnosts
        {
            get { return m_listOfVlastnost; }
            set
            {
                m_listOfVlastnost = value;
                OnPropertyChanged(nameof(ListOfVlastnosts));
            }
        }

        public PlanetDataContract SelectedPlanet
        {
            get { return m_selectedPlanet; }
            set { m_selectedPlanet = value; OnPropertyChanged(nameof(SelectedPlanet)); }
        }



        //METODY
        /// <summary>
        /// Metoda porovnava nazvy vlastnosti na PlanetDataContact.Properties s nazvy vlastnosti na Listu ListOfVlastnosts ktery uchovava vsechny mozne vlastnosti pro zobrazeni jejich nazvu.
        /// Planety nacitame v okamziku kdy se meni vyber galaxie pomoci eventu a jsou ulozeny na galaxiiDataContractu v listu.
        /// Stejne tak by moh mit PlanetaDataContract seznam vsech vlastnosti ktere je mozne zaskrtnout.
        /// </summary>
        private void CheckForCheckedProperties()
        {
            //vymazat stare hodnoty
            foreach (var dataContract in ListOfVlastnosts)
            {
                dataContract.IsChecked = false;
            }

            if (m_selectedPlanet == null)
                return;


            for (int i = 0; i < ListOfVlastnosts.Count; i++)                                        // Pro kazdou polozku ListOfVlastnosts
            {
                VlastnostDataContract vdc = ListOfVlastnosts[i];

                if (m_selectedPlanet != null)
                {

                    if (m_selectedPlanet.Properties == null)                                        // Pokud nove pridana planeta nema jeste seznam vlastnosti
                    {
                        m_selectedPlanet.Properties = new List<Vlastnost>();                        // Vytvorime ho.
                    }

                    for (int j = 0; j < m_selectedPlanet.Properties.Count; j++)                     // Projdeme seznamem vlastnosti planety,
                    {
                        Vlastnost vl = m_selectedPlanet.Properties[j];

                        if (vdc.Nazev == vl.Nazev)                                                   // Pokud se nazev nektere vlastnosti v seznamu vsech vlastnosti shoduje s nazvem vlastnosti v seznamu planety
                        {
                            vdc.IsChecked = true;                                                    // Checkni true.
                        }
                    }

                }
            }
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
            UnregisterFromEvent();

            SelectedPlanet = selectedPlanet;                              // Do vlastnosti vlozime hodnotu z parametru.
            CheckForCheckedProperties();                                  // Zkontrolujeme co je checknute


            // Pripojime posluchace eventu ktery vyhazuje VlastnostDataContract kdyz se mu zmeni nejaka vlastnost.
            if (SelectedPlanet != null)
            {
                RegisterToEvent();
            }
        }

       

        /// <summary>
        /// Odskrtavani checkboxu . Tahle metoda zachytava event ktery vyhazuje trida VlastnostDataContract kdyz se ji zmeni nejaka vlastnost. Tady IsChecked.
        /// Zapisuje do vazebni tabulky vazbu anebo ji odstranuje.
        /// Pridava nebo odebira Vlastnost do seznamu vlastnosti na SelectedPlanet. 
        /// Kliknutim na checkbox se meni vzdy jen jedna vlastnost, takze nemusime zapisovat pomoci for.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="property"></param>
        private void PropertySelectionChanged(object sender, VlastnostDataContract property)
        {
            if (property.IsChecked)
            {
                SelectedPlanet.Properties.Add(property.ConvertToDbEntity());

                VlastnostiPlanet vp = new VlastnostiPlanet();
                vp.PlanetaId = SelectedPlanet.Id;
                vp.VlastnostId = property.Id;

                m_vlastnostiPlanetDao.InsertPropertyToPlanet(vp);
            }
            else
            {
                m_vlastnostiPlanetDao.RemovePropertyFromPlanet(SelectedPlanet.Id, property.Id);

                SelectedPlanet.Properties.RemoveAll(d => d.Id == property.Id);
            }
        }


        // EVENTS AND REGISTERED METHODS
        public void UnregisterFromEvent()
        {
            foreach (var property in ListOfVlastnosts)
            {
                property.OnPropertySelectChanged -= PropertySelectionChanged;                               // Odregistrujeme posluchace
            }
        }

        public void RegisterToEvent()
        {
            foreach (var property in ListOfVlastnosts)
            {
                property.OnPropertySelectChanged += PropertySelectionChanged;                               // Odregistrujeme posluchace
            }
        }





        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
