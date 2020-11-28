using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Entities;
using Mediaresearch.Framework.DataAccess.BLToolkit.Auditable;
using Mediaresearch.Framework.DataAccess.BLToolkit.Castle;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;
using Mediaresearch.Framework.DataAccess.BLToolkit.Interceptors;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;

namespace Matika._3.Shell.Installers
{
    public class LocalhostDbAccessInstaller : IWindsorInstaller
    {
        public const string EntityDaoFactoryName = "MediaDataDaoFactory";
        public const string EntityDaoSourceName = "MediaDataDaoSource";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var configuration = container.Resolve<LocalhostDbConfiguration>();

            var entitiesAssembly = Assembly.GetAssembly(typeof(EntitiesAssemblyIdentificator))?.FullName;

            container.Register(Component.For<EntityDaoFactory>().UsingFactoryMethod(() =>
                new EntityDaoFactory(configuration.LocalhostDbAlias, configuration.LocalhostConnectionString, new TransactionManager())
                {
                    DaoAssemblies = new[] {entitiesAssembly},
                    EnumTableAssemblies = new[] {entitiesAssembly},
                    ServerTimeZoneId = configuration.ServerTimeZone,
                    Interceptors = container.ResolveAll<IDbOperationInterceptor>()
                }).LifestyleSingleton().Named(EntityDaoFactoryName));


            container.Register(Component.For<IDaoSource>().ImplementedBy<DependencyDaoSource>()
                .DependsOn(Dependency.OnComponent(typeof(EntityDaoFactory), EntityDaoFactoryName)).Named(EntityDaoSourceName).LifestyleSingleton());

            container.Register(Component.For<IDbOperationInterceptor>().ImplementedBy<AuditableInterceptor>().LifestyleSingleton());
        }
    }
    
}