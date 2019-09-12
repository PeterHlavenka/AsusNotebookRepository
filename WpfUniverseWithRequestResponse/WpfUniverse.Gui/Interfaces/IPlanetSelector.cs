using System;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Gui.Interfaces
{
    public interface IPlanetSelector
    {
        event EventHandler<PlanetDataContract> OnPlanetChanged;

        void FirePlanetChanged(PlanetDataContract planeta);

        //void SetPropertiesDependency(IEventRegistrator registrator);
    }
}