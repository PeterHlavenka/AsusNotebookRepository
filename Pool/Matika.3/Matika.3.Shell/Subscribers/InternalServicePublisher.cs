using System;
using Castle.Windsor;
using Mediaresearch.Framework.Communication.Common;

namespace Matika._3.Shell.Subscribers
{
    public class InternalServicePublisher : ClientToServicePublisherBase
    {
        private readonly IWindsorContainer m_kernel;

        public InternalServicePublisher(IWindsorContainer kernel, IServiceActionSubscriber serviceActionSubscriber) : base(serviceActionSubscriber)
        {
            m_kernel = kernel;
        }

        public override IServiceAction GetAction(Type actionType)
        {
            var action = m_kernel.Resolve(actionType);

            var actionCallback = (IServiceActionCallback) action;
            actionCallback.ExecutionFinished += ActionCallbackOnExecutionFinished;

            return (IServiceAction) action;
        }

        private void ActionCallbackOnExecutionFinished(object sender, EventArgs eventArgs)
        {
            var action = (IServiceActionCallback) sender;
            action.ExecutionFinished -= ActionCallbackOnExecutionFinished;

            m_kernel.Release(action);
        }
    }
}