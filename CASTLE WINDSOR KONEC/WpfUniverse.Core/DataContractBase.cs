using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfUniverse.Core.Annotations;

namespace WpfUniverse.Core
{
    public class DataContractBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}