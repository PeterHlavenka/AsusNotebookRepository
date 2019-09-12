namespace WpfUniverse.Common.Datacontracts
{
    public class VlastnostiPlanetDataContract 
    {
        private int m_planetaId;
        private int m_vlastnostId;

        public int VlastnostId
        {
            get => m_vlastnostId;
            set
            {
                m_vlastnostId = value;
            }
        }

        public int PlanetaId
        {
            get => m_planetaId;
            set
            {
                m_planetaId = value;
            }
        }
    }
}