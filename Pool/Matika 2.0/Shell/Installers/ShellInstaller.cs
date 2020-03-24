using System.Reflection;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Entities;
using Matika;
using Matika.Configurations;
using Matika.Gui;
using Matika.Settings;
using Mediaresearch.Framework.DataAccess.BLToolkit.Castle;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using Mediaresearch.Framework.Utilities.Configuration;

namespace Shell.Installers
{
    public class ShellInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var entities = Assembly.GetAssembly(typeof(BWord));

            container.Register(Component.For<EntityDaoFactory>().UsingFactoryMethod(() =>
                new EntityDaoFactory("matika", Properties.Settings.Default.ConnectionDb, new TransactionManager())
                {
                    DaoAssemblies = new[] {entities.FullName},
                    EnumTableAssemblies = new[] {entities.FullName},
                }));

            container.Register(Component.For<IDaoSource>().ImplementedBy<DependencyDaoSource>().LifestyleSingleton());

            container.Register(Component.For<IConfigurationProvider>().ImplementedBy<ConfigurationProvider>());
            var provider = container.Resolve<IConfigurationProvider>();
            var unitConversionConfiguration = provider.GetConfig<UnitConversionConfiguration>();
            var matikaConfiguration = provider.GetConfig<MatikaConfiguration>();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<IConvertable>().ImplementedBy<Delka>());
            container.Register(Component.For<Conversion>());
            container.Register(Component.For<MatikaViewModel>()
                .DependsOn(Dependency.OnValue("difficulty", matikaConfiguration.StartDifficulty))
                .DependsOn(Dependency.OnValue("addCount", matikaConfiguration.AddCount))
                .DependsOn(Dependency.OnValue("differenceCount", matikaConfiguration.DifferenceCount))
                .DependsOn(Dependency.OnValue("productCount", matikaConfiguration.ProductCount))
                .DependsOn(Dependency.OnValue("divideCount", matikaConfiguration.DivideCount))
            );
            container.Register(Component.For<EnumeratedWordsViewModel>());
            container.Register(Component.For<UnitConversionViewModel>().DependsOn(Dependency.OnValue("difficulty", unitConversionConfiguration.StartDifficulty))
                .DependsOn(Dependency.OnValue("stepDifference", unitConversionConfiguration.StepDifference))
            );
            container.Register(Component.For<BigNumbersSettingsViewModel>()
                .DependsOn(Dependency.OnValue("first", 100000))
                .DependsOn(Dependency.OnValue("second", 10000)));
            container.Register(Component.For<BigNumbersViewModel>());
            container.Register(Component.For<MainViewModel>());
        }
    }
}