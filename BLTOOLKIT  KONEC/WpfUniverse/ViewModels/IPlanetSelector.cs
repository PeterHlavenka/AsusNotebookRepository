using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Core;

namespace WpfUniverse.ViewModels
{
   public interface IPlanetSelector
    {
        event EventHandler<PlanetDataContract> OnPlanetChanged;

        void FirePlanetChanged(PlanetDataContract planeta);

        void SetPropertiesDependency(IEventRegistrator registrator);
    }
}
