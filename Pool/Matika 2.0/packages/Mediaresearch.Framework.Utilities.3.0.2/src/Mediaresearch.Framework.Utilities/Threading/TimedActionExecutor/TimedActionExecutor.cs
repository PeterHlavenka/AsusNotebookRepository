using System;
using System.Threading;

namespace Mediaresearch.Framework.Utilities.Threading.TimedActionExecutor
{
    public class TimedActionExecutor : ITimedActionExecutor
    {
        private Timer m_timer;
        private volatile bool m_actionRunning;
        private readonly object m_lock = new object();
        
        private Action m_action;
        private object m_context;
        private TimeSpan m_firstExecuteTimeout;
        private TimeSpan m_repeatExcecuteTimeout;
        
        public event EventHandler OnActionStarted;
        public event EventHandler OnActionCompleted;
        
        public void RegisterAction(Action action, object context, TimeSpan firstExecuteTimeout, TimeSpan repeatExcecuteTimeout)
        {
            if (m_timer != null)
            {
                Stop();
            }
            
            m_action = action;
            m_context = context;
            m_firstExecuteTimeout = firstExecuteTimeout;
            m_repeatExcecuteTimeout = repeatExcecuteTimeout;
        }

        public void Start()
        {
            if (m_action == null)
            {
                throw new InvalidOperationException("No action registered");
            }
            
            m_timer = new Timer(_ => DoAction(), m_context, m_firstExecuteTimeout, m_repeatExcecuteTimeout);
        }

        public void Stop()
        {
            m_timer?.Dispose();
            m_timer = null;
        }

        private void DoAction()
        {
            if (m_actionRunning)
            {
                return;
            }

            lock (m_lock)
            {
                try
                {
                    m_actionRunning = true;

                    FireOnActionStarted();   

                    m_action();
                }
                finally
                {
                    m_actionRunning = false;

                    FireOnActionCompleted();
                }                
            }            
        }

        private void FireOnActionStarted()
        {
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }

        private void FireOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        #region IDispose Implementation

        private bool m_disposed;

        ~TimedActionExecutor()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                }
                Stop();
            }
            m_disposed = true;
        }
        #endregion
    }
}