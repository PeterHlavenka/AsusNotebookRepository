using Mediaresearch.Framework.Communication.Common;

namespace Matika._3.Shell.Subscribers
{
    public class CustomServiceActionSubscriber
    {
        public CustomServiceActionSubscriber(IServiceActionSubscriber serviceActionSubscriber)
        {
            ServiceActionSubscriber = serviceActionSubscriber;
        }

        private IServiceActionSubscriber ServiceActionSubscriber { get; }

        public void Subscribe()
        {
        }
    }
}