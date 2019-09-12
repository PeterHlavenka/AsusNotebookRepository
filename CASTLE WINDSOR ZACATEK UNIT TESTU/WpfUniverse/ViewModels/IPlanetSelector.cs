using System;
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
