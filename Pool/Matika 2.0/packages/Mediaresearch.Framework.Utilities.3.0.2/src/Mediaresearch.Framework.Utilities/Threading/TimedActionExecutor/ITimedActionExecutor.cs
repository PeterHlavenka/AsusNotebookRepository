using System;


namespace Mediaresearch.Framework.Utilities.Threading.TimedActionExecutor
{
    public interface ITimedActionExecutor : IDisposable
    {
        event EventHandler OnActionStarted; 

        event EventHandler OnActionCompleted; 
        
        void RegisterAction(Action action, object context, TimeSpan firstExecuteTimeout, TimeSpan repeatExcecuteTimeout);

        void Start();

        void Stop();
    }
}