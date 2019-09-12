using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class GalaxyDataContract : INotifyPropertyChanged
    {
        private string m_jmeno;
        private long m_polohaX;
        private long m_polohaY;
        private long m_polohaZ;

        public GalaxyDataContract(int id, string jmeno, long polohaX, long polohaY, long polohaZ)
        {
            Id = id;
            Jmeno = jmeno;
            PolohaX = polohaX;
            PolohaY = polohaY;
            PolohaZ = polohaZ;
        }

        public int Id { get; set; }
        public string Jmeno
        {
            get { return m_jmeno; }
            set { m_jmeno = value; OnPropertyChanged(nameof(Jmeno)); }
        }
        public long PolohaX
        {
            get { return m_polohaX; }
            set { m_polohaX = value; OnPropertyChanged(nameof(PolohaX)); }
        }
        public long PolohaY
        {
            get { return m_polohaY; }
            set { m_polohaY = value; OnPropertyChanged(nameof(PolohaY)); }
        }
        public long PolohaZ
        {
            get { return m_polohaZ; }
            set { m_polohaZ = value; OnPropertyChanged(nameof(PolohaZ)); }
        }


        /// <summary>
        /// Vlastnost ktera obsahuje seznam planet v podobe PlanetDataContractu
        /// </summary>
        public List<PlanetDataContract> Planets { get; set; }





        /// <summary>
        /// Factory method. Vyrabi instance sama sebe z predane galaxie.
        /// </summary>
        /// <param name="galaxy"></param>
        /// <returns></returns>
        public static GalaxyDataContract Create(Galaxie galaxy)
        {
            return new GalaxyDataContract(galaxy.Id, galaxy.Jmeno, galaxy.PolohaX, galaxy.PolohaY, galaxy.PolohaZ);
        }

        /// <summary>
        /// Prevede GalaxyDataContract na entitu.
        /// </summary>
        /// <returns></returns>
        public Galaxie ConvertToDbEntity()
        {
            Galaxie g = new Galaxie();
            g.Id = this.Id;
            g.Jmeno = this.Jmeno;
            g.PolohaX = this.PolohaX;
            g.PolohaY = this.PolohaY;
            g.PolohaZ = this.PolohaZ;

            return g;
        }




        #region INotify Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
