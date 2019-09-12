using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Core;

namespace WpfUniverse.ViewModels
{
   public interface IGalaxySelector
    {
        event EventHandler<GalaxyDataContract> OnGalaxyChanged;

        void FireGalaxyChanged(GalaxyDataContract galaxy);
    }
}
