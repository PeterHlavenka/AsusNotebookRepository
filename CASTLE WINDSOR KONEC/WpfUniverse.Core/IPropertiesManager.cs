using System.Collections.ObjectModel;

namespace WpfUniverse.Core
{
    public interface IPropertiesManager
    {
        ObservableCollection<VlastnostDataContract> ListOfAllPossibleVlastnosts { get; set; }
    }
}