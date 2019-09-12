using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class PlanetDataContract : INotifyPropertyChanged
    {
        private int m_id;
        private string m_jmeno;
        private int m_Velikost;
        private int m_GalaxieId;
        private Guid m_identifikator;
        private List<Vlastnost> m_vlastnostiPlanet;



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
            get { return m_jmeno; }
            set { m_jmeno = value; OnPropertyChanged(nameof(Jmeno)); }
        }
        public int Velikost
        {
            get { return m_Velikost; }
            set { m_Velikost = value; OnPropertyChanged(nameof(Velikost)); }
        }
        public int GalaxieId
        {
            get { return m_GalaxieId; }
            set { m_GalaxieId = value; OnPropertyChanged(nameof(GalaxieId)); }     //neni potreba GalaxieId se nemeni
        }
        public Guid Identifikator
        {
            get { return m_identifikator; }
            set { m_identifikator = value; OnPropertyChanged(nameof(Identifikator)); }    // Identifikator by nemel jit menit.
        }


        /// <summary>
        /// Seznam Vlastnosti ktere ma tahle planeta. Vyndano z databaze pomoci join query.
        /// Bindovat muzu na PlanetDataContract ja totiz znam nazvy vlastnosti z tohoto listu. Staci metoda ktera mi vrati true pri shode.
        /// </summary>
        public List<Vlastnost> Properties
        {
            get { return m_vlastnostiPlanet; }
            set { m_vlastnostiPlanet = value; OnPropertyChanged(nameof(Properties)); }
        }
        

        /// <summary>
        /// Factory method. Vyrabi instance sama sebe z parametru planeta
        /// </summary>
        /// <param name="planeta"> Planeta podle ktere se tvori PlanetDataContract </param>
        /// <returns></returns>
        public static PlanetDataContract Create(Planeta planeta)
        {
            return new PlanetDataContract(planeta.Id, planeta.Jmeno,planeta.Velikost, planeta.GalaxieId, planeta.Identifikator, planeta.Properties);
        }


        /// <summary>
        /// Prevede DataContract na entitu.
        /// </summary>
        /// <returns></returns>
        public Planeta ConvertToDbEntity()
        {
            Planeta p = new Planeta();
            p.Id = this.Id;
            p.Jmeno = this.Jmeno;
            p.Velikost = this.Velikost;
            p.GalaxieId = this.GalaxieId;
            p.Identifikator = this.Identifikator;

            return p;
        }

        #region INotifyRegion
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        } 
        #endregion
    }
}
