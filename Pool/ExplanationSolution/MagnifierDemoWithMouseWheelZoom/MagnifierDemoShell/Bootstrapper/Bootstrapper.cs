using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using Caliburn.Micro;
using Gui.ViewModels;

namespace Shell.Bootstrapper
{
    public class Bootstrapper: Bootstrapper<MainViewModel>
    {
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetAssembly(typeof(MainViewModel))};
        }
    }
}