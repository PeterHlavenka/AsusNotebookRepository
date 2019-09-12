using System;
using System.Collections.Generic;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class PlanetDataContract : DataContractBase
    {
        private string m_jmeno;
        private int m_velikost;
        private int m_galaxieId;
        private Guid m_identifikator;
        private List<Vlastnost> m_vlastnostiPlanet;


        public PlanetDataContract()
        {

        }
         public PlanetDataContract(int id, string jmeno, int velikost, int galaxieId, Guid identifikator, List<Vlastnost> properties)
        {
            Id = id;
            Jmeno = jmeno;
            Velikost = velikost;
            GalaxieId = galaxieId;
            Identifikator = identifikator;
            Properties = properties;
        }
        

        public int Id { get; set; }
        public string Jmeno
        {
            get => m_jmeno;
            set { m_jmeno = value; OnPropertyChanged(nameof(Jmeno)); }
        }
        public int Velikost
        {
            get => m_velikost;
            set { m_velikost = value; OnPropertyChanged(nameof(Velikost)); }
        }
        public int GalaxieId
        {
            get => m_galaxieId;
            set { m_galaxieId = value; OnPropertyChanged(nameof(GalaxieId)); }    
        }
        public Guid Identifikator
        {
            get => m_identifikator;
            set { m_identifikator = value; OnPropertyChanged(nameof(Identifikator)); }    
        }      
        public List<Vlastnost> Properties
        {
            get => m_vlastnostiPlanet;
            set { m_vlastnostiPlanet = value; OnPropertyChanged(nameof(Properties)); }
        }
           
        

        public static PlanetDataContract Create(Planet planeta)
        {
            return new PlanetDataContract(planeta.Id, planeta.Name,planeta.Velikost, planeta.GalaxieId, planeta.Identifikator, planeta.Properties);
        }
   
        public Planet ConvertToDbEntity()
        {
            Planet p = new Planet
            {
                Id = Id,
                Name = Jmeno,
                Velikost = Velikost,
                GalaxieId = GalaxieId,
                Identifikator = Identifikator
            };

            return p;
        }  
    }
}
