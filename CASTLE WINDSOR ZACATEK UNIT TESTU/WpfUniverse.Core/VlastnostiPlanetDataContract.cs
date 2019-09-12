namespace WpfUniverse.Core
{
    public class VlastnostiPlanetDataContract : DataContractBase
    {
        private int m_vlastnostId;
        private int m_planetaId;

        public int VlastnostId
        {
            get => m_vlastnostId;
            set { m_vlastnostId = value; OnPropertyChanged(nameof(VlastnostId)); }
        }
        public int PlanetaId
        {
            get => m_planetaId;
            set { m_planetaId = value; OnPropertyChanged(nameof(PlanetaId)); }
        }
    }
}
