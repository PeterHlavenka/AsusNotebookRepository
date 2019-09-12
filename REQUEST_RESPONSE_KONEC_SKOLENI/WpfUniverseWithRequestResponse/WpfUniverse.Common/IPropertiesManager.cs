using System.Collections.ObjectModel;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Core
{
    public interface IPropertiesManager
    {
        ObservableCollection<VlastnostDataContract> ListOfAllPossibleVlastnosts { get; set; }
    }
}