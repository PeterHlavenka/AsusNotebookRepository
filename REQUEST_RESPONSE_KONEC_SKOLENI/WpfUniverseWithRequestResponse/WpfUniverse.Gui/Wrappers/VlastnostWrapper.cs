using System;
using Mediaresearch.Framework.Gui.Wrappers;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Gui.Wrappers
{
    public class VlastnostWrapper : ModelWrapper<VlastnostDataContract>
    {
        public VlastnostWrapper(VlastnostDataContract data) : base(data)
        {
        }

        public int Id => Data.Id;

        public string Nazev
        {
            get { return Data.Nazev; }
            set
            {
                Data.Nazev = value;
                NotifyOfPropertyChange(nameof(Nazev));
            }
        }

        private bool m_isChecked;
        public bool IsChecked
        {
            get => m_isChecked;
            set
            {
                m_isChecked = value;
                NotifyOfPropertyChange(nameof(IsChecked));                
            }
        }
        protected override void NotifyChanges()
        {
            NotifyOfPropertyChange(nameof(Id));       
            NotifyOfPropertyChange(nameof(Nazev));       
        }
    }
}