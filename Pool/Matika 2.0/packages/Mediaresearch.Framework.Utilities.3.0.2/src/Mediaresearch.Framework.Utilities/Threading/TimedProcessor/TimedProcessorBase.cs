using System;
using System.Threading;
using log4net;

namespace Mediaresearch.Framework.Utilities.Threading.TimedProcessor
{
    public abstract class TimedProcessorBase : ProcessorBase
    {
        private Timer m_doWorkTimer;

        protected static readonly ILog m_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        protected TimedProcessorBase()
        {
            DoWorkInterval = new TimeSpan(0, 0, 1);
            FirstDoWorkInterval = new TimeSpan(0, 0, 1);
        }

        protected TimedProcessorBase(TimeSpan doWorkInterval, TimeSpan firstDoWorkInterval)
        {
            DoWorkInterval = doWorkInterval;
            FirstDoWorkInterval = firstDoWorkInterval;
        }

        public TimeSpan DoWorkInterval { get; set; }
        public TimeSpan FirstDoWorkInterval { get; set; }

        protected virtual void Initialize()
        {
        }

        protected virtual void StopRequested()
        {
        }
        
        public override sealed void Start()
        {
            if (m_log.IsInfoEnabled)
                m_log.Info("Staring job timer...");
            m_doWorkTimer = new Timer(FireDoWork, null, FirstDoWorkInterval, DoWorkInterval);
            if (m_log.IsInfoEnabled)
                m_log.Info("Job timer started");

            Initialize();
        }

        public override sealed void Stop()
        {
            if (m_log.IsWarnEnabled)
                m_log.Warn("Stop requested");

            if (m_doWorkTimer != null)
            {
                if (m_log.IsInfoEnabled)
                    m_log.Info("Stopping unhandled data selection job timer...");
                m_doWorkTimer.Dispose();
            }

            StopRequested();
        }
    }
}