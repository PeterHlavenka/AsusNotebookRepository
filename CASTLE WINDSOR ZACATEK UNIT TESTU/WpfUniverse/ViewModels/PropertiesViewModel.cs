using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfUniverse.Core;
using WpfUniverse.Entities;

namespace WpfUniverse.ViewModels
{
    public class PropertiesViewModel : ViewModelBase, IEventRegistrator
    {
        private PlanetDataContract m_selectedPlanet;
        private ObservableCollection<VlastnostDataContract> m_listOfVlastnost;
        private readonly IVlastnostiPlanetDao m_vlastnostiPlanetDao;
        
        
        public PropertiesViewModel(IPlanetSelector selector, IPropertiesManager vlastnostManager, IVlastnostiPlanetDao vlastnostiPlanetDao)
        {
            m_vlastnostiPlanetDao = vlastnostiPlanetDao;
            ListOfVlastnosts = vlastnostManager.ListOfAllPossibleVlastnosts;

            selector.SetPropertiesDependency(this);
            selector.OnPlanetChanged += OnPlanetChanged;                          
        }

    
        public ObservableCollection<VlastnostDataContract> ListOfVlastnosts
        {
            get => m_listOfVlastnost;
            set
            {
                m_listOfVlastnost = value;
                OnPropertyChanged(nameof(ListOfVlastnosts));
            }
        }

        public PlanetDataContract SelectedPlanet
        {
            get => m_selectedPlanet;
            set
            {
                m_selectedPlanet = value;
                OnPropertyChanged(nameof(SelectedPlanet));
            }
        }
 
        private void CheckForCheckedProperties()
        {         
            foreach (var dataContract in ListOfVlastnosts)
            {
                dataContract.IsChecked = false;
            }

            if (m_selectedPlanet == null)
                return;


            foreach (var vdc in ListOfVlastnosts)
            {
                if (m_selectedPlanet == null) continue;

                if (m_selectedPlanet.Properties == null)                                        // Pokud nove pridana planeta nema jeste seznam vlastnosti
                {
                    m_selectedPlanet.Properties = new List<Vlastnost>();                        // Vytvorime ho.
                }

                foreach (var vl in m_selectedPlanet.Properties)
                {
                    if (vdc.Nazev == vl.Nazev)                                                   // Pokud se nazev nektere vlastnosti v seznamu vsech vlastnosti shoduje s nazvem vlastnosti v seznamu planety
                    {
                        vdc.IsChecked = true;                                                    // Checkni true.
                    }
                }
            }
        }

        
        private void OnPlanetChanged(object sender, PlanetDataContract selectedPlanet)
        {           
            UnregisterFromEvent();

            SelectedPlanet = selectedPlanet;                            
            CheckForCheckedProperties();                                 
          
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

                var vp = new VlastnostiPlanet
                {
                    PlanetaId = SelectedPlanet.Id,
                    VlastnostId = property.Id
                };

                m_vlastnostiPlanetDao.InsertPropertyToPlanet(vp);
            }
            else
            {
                m_vlastnostiPlanetDao.RemovePropertyFromPlanet(SelectedPlanet.Id, property.Id);

                SelectedPlanet.Properties.RemoveAll(d => d.Id == property.Id);
            }
        }


        
        public void UnregisterFromEvent()
        {
            foreach (var property in ListOfVlastnosts)
            {
                property.OnPropertySelectChanged -= PropertySelectionChanged;                              
            }
        }

        public void RegisterToEvent()
        {
            foreach (var property in ListOfVlastnosts)
            {
                property.OnPropertySelectChanged += PropertySelectionChanged;                             
            }
        }
    }
}
