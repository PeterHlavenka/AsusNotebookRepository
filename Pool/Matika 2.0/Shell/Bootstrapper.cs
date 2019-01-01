using System.Collections.Generic;
using System.Reflection;
using Caliburn.Micro;
using Matika;


namespace Shell
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetAssembly(typeof(MainViewModel))};
        }
    }
}