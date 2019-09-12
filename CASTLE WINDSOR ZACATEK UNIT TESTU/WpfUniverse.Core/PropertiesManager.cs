using System.Collections.ObjectModel;
using System.Linq;
using WpfUniverse.Entities;

namespace WpfUniverse.Core
{
   public class PropertiesManager : IPropertiesManager
    {
        public PropertiesManager(IVlastnostDao vlastnostDao)
        {
            ListOfAllPossibleVlastnosts = new ObservableCollection<VlastnostDataContract>(vlastnostDao.SelectAll().Select(VlastnostDataContract.Create).ToList());
        }

      
        public  ObservableCollection<VlastnostDataContract> ListOfAllPossibleVlastnosts { get; set; }
    }
}
