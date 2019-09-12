using System;
using WpfUniverse.Core;

namespace WpfUniverse.ViewModels
{
    public interface IGalaxySelector
    {
        event EventHandler<GalaxyDataContract> OnGalaxyChanged;

        void FireGalaxyChanged(GalaxyDataContract galaxy);
    }
}