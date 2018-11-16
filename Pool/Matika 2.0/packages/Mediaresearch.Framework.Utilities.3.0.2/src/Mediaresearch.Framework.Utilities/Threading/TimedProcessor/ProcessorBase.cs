using System;
using System.Threading;
using log4net;

namespace Mediaresearch.Framework.Utilities.Threading.TimedProcessor
{
    public abstract class ProcessorBase 
    {
        private static readonly ILog m_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        private bool m_disposed;
        private readonly object m_doWorkLock = new object();

        public abstract void Start();
        public abstract void Stop();

        protected virtual void ReleaseManagedResources()
        { }

        protected virtual void ReleaseUnmanagedResources()
        { }

        ~ProcessorBase()
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
                    ReleaseManagedResources();
                }

                ReleaseUnmanagedResources();
            }
            m_disposed = true;
        }

        protected abstract void DoWork();

        protected void FireDoWork(object arg)
        {
            if (Monitor.TryEnter(m_doWorkLock))
            {
                try
                {
                    DoWork();
                }
                catch (Exception ex)
                {
                    if (m_log.IsFatalEnabled)
                        m_log.Fatal("Unhandled exception occured!", ex);
                    throw;
                }
                finally
                {
                    Monitor.Exit(m_doWorkLock);
                }
            }
        }

        internal void FireDoWork()
        {
            FireDoWork(null);
        }
    }
}