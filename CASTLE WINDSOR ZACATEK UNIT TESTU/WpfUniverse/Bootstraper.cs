using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;
using System.Reflection;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using Castle.MicroKernel.SubSystems.Configuration;
using WpfUniverse.Entities;
using Mediaresearch.Framework.DataAccess.BLToolkit.Castle;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using WpfUniverse.Core;
using WpfUniverse.ViewModels;

namespace WpfUniverse
{
    public class Bootstraper
    {
        public void Start()
        {       
            CastleContainer.Container.Install(new Installer());                               // Trida.Vlastnost(vraci IWindsorContainer).MetodaWindsorContaineru
            CastleContainer.Container.Resolve<IDaoSource>();
        }
    }

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Assembly entities = Assembly.GetAssembly(typeof(Galaxie));

            container.Register(Component.For<EntityDaoFactory>().UsingFactoryMethod(() =>
                new EntityDaoFactory("universe", Properties.Settings.Default.ConnectionDb, new TransactionManager())
                {
                    DaoAssemblies = new[] { entities.FullName },
                    EnumTableAssemblies = new[] { entities.FullName },
                }));

            container.Register(Component.For<IDaoSource>().ImplementedBy<DependencyDaoSource>().LifestyleSingleton());


            container.Register(Component.For<GalaxyViewModel>().ImplementedBy<GalaxyViewModel>().Forward<IGalaxySelector>().LifestyleSingleton());
            container.Register(Component.For<PlanetsViewModel>().ImplementedBy<PlanetsViewModel>().Forward<IPlanetSelector>().LifestyleSingleton());
            container.Register(Component.For<IPropertiesManager>().ImplementedBy<PropertiesManager>().LifestyleSingleton());
            container.Register(Component.For<PropertiesViewModel>().ImplementedBy<PropertiesViewModel>().LifestyleSingleton());            
        }
    }
}
