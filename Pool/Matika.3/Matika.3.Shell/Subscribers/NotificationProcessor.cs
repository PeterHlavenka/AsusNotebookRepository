using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Mediaresearch.Framework.Communication.Common;

namespace Matika._3.Shell.Subscribers
{
    public class NotificationProcessor : NotificationReceiverProcessor
    {
        public NotificationProcessor(INotificationsReceiversAssemblyProvider notificationsReceiversAssemblyProvider, IWindsorContainer container) : base(notificationsReceiversAssemblyProvider, container)
        {
        }

        protected override void DoAfterSubscribe<TNotification>(Type receiverType)
        {
            if (!m_container.Kernel.HasComponent(receiverType)) m_container.Register(Component.For(receiverType).ImplementedBy(receiverType).LifestyleSingleton());
        }
    }
}