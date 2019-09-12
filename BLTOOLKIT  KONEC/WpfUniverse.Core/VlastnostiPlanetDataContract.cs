using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class VlastnostiPlanetDataContract : INotifyPropertyChanged
    {
        private int m_vlastnostId;
        private int m_planetaId;

        public int VlastnostId
        {
            get { return m_vlastnostId; }
            set { m_vlastnostId = value; OnPropertyChanged(nameof(VlastnostId)); }
        }

        public int PlanetaId
        {
            get { return m_planetaId; }
            set { m_planetaId = value; OnPropertyChanged(nameof(PlanetaId)); }
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
