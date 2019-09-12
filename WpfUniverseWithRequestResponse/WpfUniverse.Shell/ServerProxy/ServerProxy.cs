using System;
using Castle.Windsor;
using Mediaresearch.Framework.Communication.Common;

namespace WpfUniverse.Shell.ServerProxy
{
    public class ServerProxy : ClientToServicePublisherBase
    {
        private readonly IWindsorContainer m_container;

        public ServerProxy(IServiceActionSubscriber serviceActionSubscriber, IWindsorContainer container) : base(serviceActionSubscriber)
        {
            m_container = container;
        }

        public override IServiceAction GetAction(Type actionType)
        {
            return (IServiceAction) m_container.Resolve(actionType);
        }
    }
}