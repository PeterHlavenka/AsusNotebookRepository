using System.Collections.Generic;

namespace WpfUniverse.Common.Datacontracts
{
    public class GalaxyDataContract
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
            set
            {
                m_jmeno = value;
            }
        }

        public long PolohaX
        {
            get => m_polohaX;
            set
            {
                m_polohaX = value;
            }
        }

        public long PolohaY
        {
            get => m_polohaY;
            set
            {
                m_polohaY = value;
            }
        }

        public long PolohaZ
        {
            get => m_polohaZ;
            set
            {
                m_polohaZ = value;
            }
        }


        public List<PlanetDataContract> Planets { get; set; }

        
        //public static GalaxyDataContract Create(Galaxy galaxy)
        //{
        //    return new GalaxyDataContract(galaxy.Id, galaxy.Jmeno, galaxy.PolohaX, galaxy.PolohaY, galaxy.PolohaZ);
        //}

        //public Galaxy ConvertToDbEntity()
        //{
        //    var g = new Galaxy
        //    {
        //        Id = Id,
        //        Jmeno = Jmeno,
        //        PolohaX = PolohaX,
        //        PolohaY = PolohaY,
        //        PolohaZ = PolohaZ
        //    };

        //    return g;
        //}
    }
}