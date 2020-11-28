using System;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Matika._3.Gui;
using Matika._3.Shell.Subscribers;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.DataAccess.Core.Auditable;

namespace Matika._3.Shell.Installers
{
    public class ShellInstaller : IWindsorInstaller
    {
        public ShellInstaller()
        {
            ShellInstallerKey = Guid.NewGuid().ToString();
        }

        private static string ShellInstallerKey { get; set; }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var configuration = container.Resolve<LocalhostDbConfiguration>();

            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>().Named(ShellInstallerKey));
            container.Register(Component.For<IAuditableIdentityProvider>().ImplementedBy<SimpleAuditableEntityProvider>());
            container.Register(Component.For<ITimeProvider>().UsingFactoryMethod(() => new TimeZoneTimeProvider(configuration.ServerTimeZone)).LifestyleSingleton());
            
            // ServicePublisher
            container.Register(Component.For<IRequestsServiceActionsAssemblyProvider>().ImplementedBy<RequestsServiceActionsAssemblyProvider>());
            container.Register(Component.For<IServiceActionSubscriber>().ImplementedBy<ServiceActionSubscriber>());
            container.Register(Component.For<INotificationsReceiversAssemblyProvider>().ImplementedBy<NotificationsReceiversAssemblyProvider>());
            container.Register(Component.For<INotificationReceiverProcessor>().ImplementedBy<NotificationProcessor>().Forward<INotificationReceiverSubscriber>());
            container.Register(Component.For<IClientToServicePublisher>().ImplementedBy<InternalServicePublisher>().LifestyleSingleton());
            
            // Registrace custom subscribera
            container.Register(Component.For<CustomServiceActionSubscriber>().ImplementedBy<CustomServiceActionSubscriber>());

            container.Register(Component.For<MainViewModel>().LifestyleSingleton());
        }
    }
}