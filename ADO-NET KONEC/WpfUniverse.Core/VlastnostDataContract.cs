using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class VlastnostDataContract : INotifyPropertyChanged
    {
        private int m_id;
        private string m_nazev;
        private bool m_isChecked;

//KONSTRUKTORY
        public VlastnostDataContract() { }

        public VlastnostDataContract(int id, string nazev)
        {
            Id = id;
            Nazev = nazev;
        }

//VLASTNOSTI
        public int Id
        {
            get { return m_id; }
            set { m_id = value; OnPropertyChanged(nameof(Id)); }
        }
        public string Nazev
        {
            get { return m_nazev; }
            set { m_nazev = value; OnPropertyChanged(nameof(Nazev)); }
        }

        /// <summary>
        /// Na tuto vlastnost je bindovano checknuti checkboxu. 
        /// PropertiesViewModel ma kolekci VlastnostDataContractu a binduje SelectedItem na IsChecked.
        /// </summary>
        public bool IsChecked
        {
            get { return m_isChecked; }
            set
            {
                m_isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
               
                FireSelectionChanged();
            }
        }


//METODY
        /// <summary>
        /// Factory method. Vyrabi instance sama sebe z parametru vlastnost.
        /// </summary>
        /// <param name="vlastnost"></param>
        /// <returns></returns>
        public static VlastnostDataContract Create(Vlastnost vlastnost)
        {
            return new VlastnostDataContract(vlastnost.Id, vlastnost.Nazev);
        }


        /// <summary>
        /// Prevod na Vlastnost
        /// </summary>
        /// <returns></returns>
        public Vlastnost ConvertToDbEntity()
        {
            Vlastnost vlastnost = new Vlastnost();

            vlastnost.Id = Id;
            vlastnost.Nazev = Nazev;

            return vlastnost;
        }

//EVENT
        public event EventHandler<VlastnostDataContract> OnPropertySelectChanged;
        private void FireSelectionChanged()
        {
            OnPropertySelectChanged?.Invoke(this, this);
        }

//INOTIFY
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
