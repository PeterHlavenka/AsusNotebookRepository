using System;
using System.Collections.Generic;

namespace WpfUniverse.Common.Datacontracts
{
    public class PlanetDataContract
    {
        private int m_galaxieId;
        private Guid m_identifikator;
        private string m_jmeno;
        private int m_velikost;
        
        public PlanetDataContract()
        {
        }

        public PlanetDataContract(int id, string jmeno, int velikost, int galaxieId, Guid identifikator)
        {
            Id = id;
            Jmeno = jmeno;
            Velikost = velikost;
            GalaxieId = galaxieId;
            Identifikator = identifikator;
        }


        public int Id { get; set; }

        public string Jmeno
        {
            get => m_jmeno;
            set
            {
                m_jmeno = value;
            }
        }

        public int Velikost
        {
            get => m_velikost;
            set
            {
                m_velikost = value;
            }
        }

        public int GalaxieId
        {
            get => m_galaxieId;
            set
            {
                m_galaxieId = value;
            }
        }

        public Guid Identifikator
        {
            get => m_identifikator;
            set
            {
                m_identifikator = value;
            }
        }

        //public List<Vlastnost> Properties
        //{
        //    get => m_vlastnostiPlanet;
        //    set
        //    {
        //        m_vlastnostiPlanet = value;
        //        OnPropertyChanged(nameof(Properties));
        //    }
        //}


        //public static PlanetDataContract Create(Planet planeta)
        //{
        //    return new PlanetDataContract(planeta.Id, planeta.Name, planeta.Velikost, planeta.GalaxieId,
        //        planeta.Identifikator, planeta.Properties);
        //}

        //public Planet ConvertToDbEntity()
        //{
        //    var p = new Planet
        //    {
        //        Id = Id,
        //        Name = Jmeno,
        //        Velikost = Velikost,
        //        GalaxieId = GalaxieId,
        //        Identifikator = Identifikator
        //    };

        //    return p;
        //}
    }
}