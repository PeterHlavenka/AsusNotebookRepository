using System;
using System.Collections.Generic;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Gui.Interfaces
{
    public interface IGalaxySelector
    {
        event EventHandler<List<GalaxyDataContract>> OnGalaxiesChanged;

        void FireGalaxiesChanged(List<GalaxyDataContract> galaxies);

        
    }
}