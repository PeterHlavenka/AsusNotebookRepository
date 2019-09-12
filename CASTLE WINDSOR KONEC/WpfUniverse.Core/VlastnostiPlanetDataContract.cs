namespace WpfUniverse.Core
{
    public class VlastnostiPlanetDataContract : DataContractBase
    {
        private int m_planetaId;
        private int m_vlastnostId;

        public int VlastnostId
        {
            get => m_vlastnostId;
            set
            {
                m_vlastnostId = value;
                OnPropertyChanged(nameof(VlastnostId));
            }
        }

        public int PlanetaId
        {
            get => m_planetaId;
            set
            {
                m_planetaId = value;
                OnPropertyChanged(nameof(PlanetaId));
            }
        }
    }
}