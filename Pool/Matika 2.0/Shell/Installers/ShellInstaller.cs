using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Matika;
using Matika.Configurations;
using Mediaresearch.Framework.Utilities.Configuration;

namespace Shell.Installers
{
    public class ShellInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationProvider>().ImplementedBy<ConfigurationProvider>());
            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());
            container.Register(Component.For<MainViewModel>());

            //var provider = container.Resolve<IConfigurationProvider>();
            //var test = provider.GetConfig<MatikaConfiguration>().Test;
        }
    }
}