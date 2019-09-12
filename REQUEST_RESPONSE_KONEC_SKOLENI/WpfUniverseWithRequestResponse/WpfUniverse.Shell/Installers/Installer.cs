using System.Reflection;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.DataAccess.BLToolkit.Castle;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using WpfUniverse.Core;
using WpfUniverse.Gui.Interfaces;
using WpfUniverse.Gui.ViewModels;
using WpfUniverse.Shell.Properties;

namespace WpfUniverse.Shell.Installers
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<EntityDaoFactory>().UsingFactoryMethod(() =>
                new EntityDaoFactory("universe", Settings.Default.ConnectionDb, new TransactionManager())
                {
                    DaoAssemblies = new[] { "WpfUniverse.Entities" },
                    EnumTableAssemblies = new[] { "WpfUniverse.Entities" }
                }));

            container.Register(Component.For<IDaoSource>().ImplementedBy<DependencyDaoSource>().LifestyleSingleton());
            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifestyleSingleton());


            container.Register(Component.For<GalaxyViewModel>().ImplementedBy<GalaxyViewModel>().Forward<IGalaxySelector>().LifestyleSingleton());
            container.Register(Component.For<PlanetsViewModel>().ImplementedBy<PlanetsViewModel>().Forward<IPlanetSelector>().LifestyleSingleton());
            container.Register(Component.For<PropertiesViewModel>().ImplementedBy<PropertiesViewModel>());
            container.Register(Component.For<MainViewModel>());
            container.Register(Component.For<GalaxyDialogViewModel>().ImplementedBy<GalaxyDialogViewModel>());
            container.Register(Component.For<PlanetsDialogViewModel>().ImplementedBy<PlanetsDialogViewModel>());
            container.Register(Component.For<EditPropertyViewModel>().ImplementedBy<EditPropertyViewModel>());

            container.Register(Component.For<IClientToServicePublisher>().ImplementedBy<ServerProxy.ServerProxy>());
            container.Register(Component.For<IRequestsServiceActionsAssemblyProvider>().ImplementedBy<RequestsServiceActionsAssemblyProvider>());
            container.Register(Component.For<IServiceActionSubscriber>().ImplementedBy<ServiceActionSubscriber>()
                .OnCreate(d => d.SubscribeAll()));
        }
    }
}