using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Matika;

namespace Shell.Installers
{
    public class ShellInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());
            container.Register(Component.For<MainViewModel>());
        }
    }
}