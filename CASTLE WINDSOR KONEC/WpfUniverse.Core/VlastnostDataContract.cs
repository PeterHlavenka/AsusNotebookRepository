using System;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
    public class VlastnostDataContract : DataContractBase
    {
        private int m_id;
        private bool m_isChecked;
        private string m_nazev;


        public VlastnostDataContract()
        {
        }

        public VlastnostDataContract(int id, string nazev)
        {
            Id = id;
            Nazev = nazev;
        }


        public int Id
        {
            get => m_id;
            set
            {
                m_id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Nazev
        {
            get => m_nazev;
            set
            {
                m_nazev = value;
                OnPropertyChanged(nameof(Nazev));
            }
        }

        public bool IsChecked
        {
            get => m_isChecked;
            set
            {
                m_isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                FireSelectionChanged();
            }
        }


        public static VlastnostDataContract Create(Vlastnost vlastnost)
        {
            return new VlastnostDataContract(vlastnost.Id, vlastnost.Nazev);
        }

        public Vlastnost ConvertToDbEntity()
        {
            var vlastnost = new Vlastnost
            {
                Id = Id,
                Nazev = Nazev
            };


            return vlastnost;
        }


        public event EventHandler<VlastnostDataContract> OnPropertySelectChanged;

        private void FireSelectionChanged()
        {
            OnPropertySelectChanged?.Invoke(this, this);
        }
    }
}