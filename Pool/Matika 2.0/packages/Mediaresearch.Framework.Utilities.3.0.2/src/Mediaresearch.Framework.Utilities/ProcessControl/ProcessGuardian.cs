using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using log4net;

namespace Mediaresearch.Framework.Utilities.ProcessControl
{
    public class ProcessGuardian : IProcessGuardian, IDisposable
    {
	    private readonly Func<int> m_processMemoryUsageLimitInMegaBytesFunc;
	    public static string[] ProcesseNames = {"avconv", "ffmpeg", "PeaksCreator", "VfpCreator", "avprobe", "vlc", "gst-launch-1.0"};
        
        private static readonly ILog m_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object m_synchRoot = new object();
        private readonly List<ProcessWraper> m_cache = new List<ProcessWraper>();
        private readonly Timer m_guardTimer;
        private volatile bool m_checkIsRunning;

        public ProcessGuardian(Func<int> processMemoryUsageLimitInMegaBytesFunc)
        {
	        m_processMemoryUsageLimitInMegaBytesFunc = processMemoryUsageLimitInMegaBytesFunc;
	        m_guardTimer = new Timer(_ => CheckProcesses(), null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

	    public void AddProcess(ProcessWraper processWraper)
        {
            lock (m_synchRoot)
            {
                m_cache.Add(processWraper);
            }
        }

        public void RemoveProcess(ProcessWraper processWraper)
        {
            lock (m_synchRoot)
            {
                m_cache.Remove(processWraper);
                processWraper.Dispose();
            }
        }

        public void KillAllProcess()
        {
            m_log.WarnFormat("Killing all runnig processes");

            lock (m_synchRoot)
            {
                foreach (var wraper in m_cache)
                {
                    wraper.Kill();
                }
            }
        }

        public void KillAllProcessExecutedFromPreviousServiceInstance()
        {
            foreach (var processName in ProcesseNames)
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    KillProcess(processName, process);
                }
            }
        }

        public void KillAllProcess(string[] fullNames)
        {
            foreach (var fullName in fullNames)
            {
                string processName = Path.GetFileName(fullName);
                var processes = Process.GetProcessesByName(processName);

                var sameProcesses = processes.Where(d => d.MainModule.FileName.Equals(fullName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                foreach (var process in sameProcesses)
                {
                    KillProcess(processName, process);
                }
            }          

        }

        private static void KillProcess(string processName, Process process)
        {
            try
            {
                m_log.InfoFormat("Killing running process '{0}'", processName);
                process.Kill();
            }
            catch (Exception ex)
            {
                m_log.Error("Can't kill running process", ex);
            }
        }

        private void CheckProcesses()
        {
            if (m_checkIsRunning)
                return;

            try
            {
                if (m_log.IsDebugEnabled)
                    m_log.Debug("Running memory check of processes...");

                m_checkIsRunning = true;

                List<ProcessWraper> cacheCopy;
                lock (m_synchRoot)
                {
                    cacheCopy = m_cache.ToList();
                }

                int encoderMemoryUsageLimitInMegaBytes = m_processMemoryUsageLimitInMegaBytesFunc();

                foreach (var wraper in cacheCopy)
                {
                    try
                    {
                        if (!wraper.HasExited)
                        {
                            double encoderMemoryUsageInMegaBytes = wraper.GetEncoderMemoryUsageInMegaBytes();

                            m_log.InfoFormat("Current memory usage of process '{0}' is '{1}MB'", wraper.ProcessName, encoderMemoryUsageInMegaBytes);

                            if (encoderMemoryUsageInMegaBytes > encoderMemoryUsageLimitInMegaBytes)
                            {
                                string message = $"Actual memory usage '{encoderMemoryUsageInMegaBytes}MB' of encoder process is greaher than limit '{encoderMemoryUsageLimitInMegaBytes}MB'. Killing process...";
                                m_log.WarnFormat(message);
                                wraper.IgnoreErrorOutput = true;
                                wraper.AlternativeErrorOutput = message;
                                wraper.Kill();
                            }
                        }
                        else
                        {
                            m_log.Info("Procces exited. Memory check isn't necessary");
                        }
                    }
                    catch (Exception e)
                    {
                        m_log.Warn("Can't check procces exited. Removing process from collection", e);
                        lock (m_synchRoot)
                        {
                            m_cache.Remove(wraper);
                        }
                    }
                }
            }
            finally
            {
                m_checkIsRunning = false;
            }
        }


        #region IDispose Implementation

        private bool m_disposed;

        ~ProcessGuardian()
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
                if (m_guardTimer != null)
                    m_guardTimer.Dispose();
            }
            m_disposed = true;
        }

        #endregion
    }
}