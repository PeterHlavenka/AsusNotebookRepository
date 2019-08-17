using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Matika;
using Matika.Configurations;
using Matika.Gui;
using Mediaresearch.Framework.Utilities.Configuration;

namespace Shell.Installers
{
    public class ShellInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationProvider>().ImplementedBy<ConfigurationProvider>());
            var provider = container.Resolve<IConfigurationProvider>();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());
            container.Register(Component.For<IConvertable>().ImplementedBy<Delka>());
            container.Register(Component.For<Conversion>());
            container.Register(Component.For<MatikaViewModel>().DependsOn(Dependency.OnValue("difficulty", provider.GetConfig<MatikaConfiguration>().StartDifficulty)));
            container.Register(Component.For<EnumeratedWordsViewModel>());
            container.Register(Component.For<UnitConversionViewModel>().DependsOn(Dependency.OnValue("difficulty", provider.GetConfig<UnitConversionConfiguration>().StartDifficulty)));
            container.Register(Component.For<MainViewModel>());
        }
    }
}