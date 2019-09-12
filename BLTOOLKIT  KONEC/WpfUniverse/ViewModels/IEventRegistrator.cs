using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.ViewModels
{
    public interface IEventRegistrator
    {
        void UnregisterFromEvent();
        void RegisterToEvent();
    }
}
