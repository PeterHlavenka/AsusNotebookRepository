using System.Collections.Generic;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class GalaxyDataContract : DataContractBase
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
            get => m_jmeno;
            set { m_jmeno = value; OnPropertyChanged(nameof(Jmeno)); }
        }
        public long PolohaX
        {
            get => m_polohaX;
            set { m_polohaX = value; OnPropertyChanged(nameof(PolohaX)); }
        }
        public long PolohaY
        {
            get => m_polohaY;
            set { m_polohaY = value; OnPropertyChanged(nameof(PolohaY)); }
        }
        public long PolohaZ
        {
            get => m_polohaZ;
            set { m_polohaZ = value; OnPropertyChanged(nameof(PolohaZ)); }
        }


       
        public List<PlanetDataContract> Planets { get; set; }


       
        public static GalaxyDataContract Create(Galaxie galaxy)
        {
            return new GalaxyDataContract(galaxy.Id, galaxy.Jmeno, galaxy.PolohaX, galaxy.PolohaY, galaxy.PolohaZ);
        }

        public Galaxie ConvertToDbEntity()
        {
            var g = new Galaxie
            {
                Id = this.Id,
                Jmeno = this.Jmeno,
                PolohaX = this.PolohaX,
                PolohaY = this.PolohaY,
                PolohaZ = this.PolohaZ
            };

            return g;
        }
    }
}
