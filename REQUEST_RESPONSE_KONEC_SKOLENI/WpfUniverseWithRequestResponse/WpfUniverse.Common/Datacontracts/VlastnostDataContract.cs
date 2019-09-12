using System;
using System.Collections.Generic;

namespace WpfUniverse.Common.Datacontracts
{
    public class VlastnostDataContract
    {
        private int m_id;
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
            }
        }

        public string Nazev
        {
            get => m_nazev;
            set
            {
                m_nazev = value;
            }
        }

        //public List<string> Items { get; set; } = new List<string>{ "test1", "test2", "test3" };


        //public static VlastnostDataContract Create(Vlastnost vlastnost)
        //{
        //    return new VlastnostDataContract(vlastnost.Id, vlastnost.Nazev);
        //}

        //public Vlastnost ConvertToDbEntity()
        //{
        //    var vlastnost = new Vlastnost
        //    {
        //        Id = Id,
        //        Nazev = Nazev
        //    };


        //    return vlastnost;
        //}


        //public event EventHandler<VlastnostDataContract> OnPropertySelectChanged;

        //private void FireSelectionChanged()
        //{
        //    OnPropertySelectChanged?.Invoke(this, this);
        //}
    }
}