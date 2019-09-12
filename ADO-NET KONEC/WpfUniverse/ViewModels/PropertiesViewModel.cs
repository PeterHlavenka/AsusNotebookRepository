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
    public class PropertiesViewModel : INotifyPropertyChanged
    {
        private PlanetsViewModel m_planetsViewModel;
        private PlanetDataContract m_selectedPlanet;
        private ObservableCollection<VlastnostDataContract> m_listOfVlastnost;
        private VlastnostDataContract m_vlastnostDataContract;
        private VlastnostiPlanetDao m_vlastnostiPlanetDao;
        private VlastnostDao m_vlastnostDao;

        //KONSTRUKTORY
        public PropertiesViewModel(PlanetsViewModel planetsViewModel)
        {
            m_planetsViewModel = planetsViewModel;
            m_planetsViewModel.OnPlanetChanged += OnPlanetChanged;
            m_vlastnostDataContract = new VlastnostDataContract();
            m_vlastnostiPlanetDao = new VlastnostiPlanetDao(MainWindow.Conn);
            m_vlastnostDao = new VlastnostDao(MainWindow.Conn);


            ListOfVlastnosts = new ObservableCollection<VlastnostDataContract>(m_vlastnostDao.LoadAllVlastnosts().Select(x => VlastnostDataContract.Create(x)).ToList());

        }

        //VLASTNOSTI
        /// <summary>
        /// Do teto kolekce se nactou vsechny vlastnosti , abychom mohli zobrazit vsechny vlastnosti checknute i nechecknute.
        /// </summary>
        public ObservableCollection<VlastnostDataContract> ListOfVlastnosts
        {
            get { return m_listOfVlastnost; }
            set
            {
                m_listOfVlastnost = value;
                OnPropertyChanged(nameof(ListOfVlastnosts));
                Console.WriteLine("ListOfVlastnosts.set => meni se jen pri zmene poctu polozek listu. ");
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="selectedPlanet"></param>
        private void OnPlanetChanged(object sender, PlanetDataContract selectedPlanet)
        {
            Console.WriteLine("PropertiesViewModel.OnPlanetChanged => m_selectedPlanet  se zmenila");

            foreach (var property in ListOfVlastnosts)
            {
                property.OnPropertySelectChanged -= PropertyOnPlanetSelectionChanged;                               // Odregistrujeme posluchace
            }

            SelectedPlanet = selectedPlanet;
            CheckForCheckedProperties();

            // Zkontroluj co je checknute

            if (SelectedPlanet != null)
            {
                foreach (var property in ListOfVlastnosts)
                {
                    property.OnPropertySelectChanged += PropertyOnPlanetSelectionChanged;                               // A zaregistrujeme zpet
                }
            }
        }


        /// <summary>
        /// Zapisuje do vazebni tabulky vazbu anebo ji odstranuje.
        /// Pridava nebo odebira Vlastnost do seznamu vlastnosti na SelectedPlanet. Na tyto vlastnosti je bindovano.
        /// Kliknutim na checkbox se meni vzdy jen jedna vlastnost, takze nemusime zapisovat pomoci for.
        /// Zmena v databazi se neprojevi na promenne VlastnostDataContract property.
        /// Musime ji zmenit aby se nam refreshlo View.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="property"></param>
        private void PropertyOnPlanetSelectionChanged(object sender, VlastnostDataContract property)
        {
            if (property.IsChecked)
            {
                SelectedPlanet.Properties.Add(property.ConvertToDbEntity());

                m_vlastnostiPlanetDao.InserPropertyToPlanet(SelectedPlanet.Id, property.Id);
            }
            else
            {
                m_vlastnostiPlanetDao.RemovePropertyFromPlanet(SelectedPlanet.Id, property.Id);

                SelectedPlanet.Properties.RemoveAll(d => d.Id == property.Id);
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
