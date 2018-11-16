using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using log4net;

namespace Mediaresearch.Framework.Utilities.ProcessControl
{
    public class ProcessWraper : IDisposable, IEquatable<ProcessWraper>
    {
        private static readonly ILog m_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);
        delegate bool ConsoleCtrlDelegate(uint ctrlType);

        private readonly object m_synchRoot = new object();
        private readonly Guid m_id = Guid.NewGuid();

        public event EventHandler Exited;

        public event EventHandler<DataReceivedEventArgs> OnStandartOutput;

        public event EventHandler<DataReceivedEventArgs> OnErrorOutput;

        public Process Process { get; }

        public bool IgnoreErrorOutput { get; set; }

        public string AlternativeErrorOutput { get; set; }

        public string ProcessName { get; private set; }

        public Guid UniqueId => m_id;

        public object Context { get; set; }

        public ProcessPriorityClass PriorityClass
        {
            get
            {
                lock (m_synchRoot)
                {
                    if (Process == null)
                        return ProcessPriorityClass.Normal;

                    return Process.PriorityClass;                    
                }
            }
            set
            {
                lock (m_synchRoot)
                {
                    Process.PriorityClass = value;                    
                }
            }
        }

        public bool HasExited
        {
            get
            {
                lock (m_synchRoot)
                {
                    if (Process == null)
                        return true;

                    return Process.HasExited;                    
                }
            }
        }

        public int ExitCode
        {
            get
            {
                lock (m_synchRoot)
                {
                    if (Process == null)
                        return -1;

                    return Process.ExitCode;                    
                }
            }
        }

        public ProcessWraper(ProcessStartInfo processStartInfo)
        {
            Process = new Process();
            Process.StartInfo = processStartInfo;

            if (processStartInfo.RedirectStandardOutput)
            {
                Process.OutputDataReceived += OnProcessOnOutputDataReceived;
            }

            if (processStartInfo.RedirectStandardError)
            {
                Process.ErrorDataReceived += OnProcessOnErrorDataReceived;
            }
        }

        private void OnProcessOnErrorDataReceived(object sender, DataReceivedEventArgs args)
        {
            FireErrorOutput(args);
        }

        private void OnProcessOnOutputDataReceived(object sender, DataReceivedEventArgs args)
        {
            FireStandartOutput(args);
        }

        public void Start()
        {
            lock (m_synchRoot)
            {
                Process.EnableRaisingEvents = true;
                Process.Exited += ProcessOnExited;
                Process.Start();

                if (Process.StartInfo.RedirectStandardOutput)
                {
                    Process.BeginOutputReadLine();
                }
                if (Process.StartInfo.RedirectStandardError)
                {
                    Process.BeginErrorReadLine();
                }

                try
                {
                    ProcessName = Process.ProcessName;
                }
                catch (InvalidOperationException)
                {
                    ProcessName = "unknown";
                }
            }
        }

        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            OnExited();
        }

        public void Stop(string stopcCommand, TimeSpan stopTimeout, bool needUseCtrlCEvent = false)
        {
            try
            {
                lock (m_synchRoot)
                {
                    if (Process != null && !Process.HasExited)
                    {
                        if (!string.IsNullOrWhiteSpace(stopcCommand))
                        {
                            var input = Process.StandardInput;
                            input.WriteLine(stopcCommand);
                            input.Close();
                        }

                        if (needUseCtrlCEvent)
                        {
                            if (AttachConsole((uint) Process.Id))
                            {
                                SetConsoleCtrlHandler(null, true);
                                try
                                {
                                    if (GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                                    {
                                        Process.WaitForExit((int)stopTimeout.TotalMilliseconds);
                                    }
                                }
                                finally
                                {
                                    FreeConsole();
                                    SetConsoleCtrlHandler(null, false);
                                }
                            }
                        }
                        else
                        {
                            Process.WaitForExit((int)stopTimeout.TotalMilliseconds);
                        }

                        if (!Process.HasExited)
                        {
                            m_log.Warn("Process stop timeout expired, killing process");
                            Process.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_log.Error("Stopping process error", ex);
            }
        }

        public void Kill()
        {
            lock (m_synchRoot)
            {
                if (Process != null && !Process.HasExited)
                    Process.Kill();                            
            }
        }

        public void WaitForExit(TimeSpan timeout)
        {
            if (Process != null && !Process.HasExited)
                Process.WaitForExit((int)timeout.TotalMilliseconds);
        }

        public double GetEncoderMemoryUsageInMegaBytes()
        {
            lock (m_synchRoot)
            {
                int handleCount = -1;
                bool? exited = null;
                
                try
                {
                    if (Process == null)
                        return 0;

                    handleCount = Process.HandleCount;
                    exited = Process.HasExited;

                    m_log.Info($"Checking memory of process {Process.Id} {ProcessName}");

                    using (var performanceCounter = new PerformanceCounter("Process", "Working Set - Private", GetProcessInstanceName(Process.Id)))
                    {
                        long memoryUsage = performanceCounter.RawValue;

                        return memoryUsage/1024D/1024D;
                    }
                }
                catch (Exception ex)
                {
                    m_log.WarnFormat("Can't resolve current memory usage of process, returning zero. Handles count '{0}'; Exited '{1}'; Error:{2} ", handleCount, exited, ex.Message);
                    return 0;
                }
            }
        }

        private string GetProcessInstanceName(int pid)
        {
             PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Process");
             string[] instances = performanceCounterCategory.GetInstanceNames();
            foreach (string instance in instances)
            {
                using (PerformanceCounter cnt = new PerformanceCounter("Process","ID Process", instance, true))
                {
                    int val = (int)cnt.RawValue;
                    if (val == pid)
                    {
                        return instance;
                    }
                }
            }

            throw new Exception("Could not find performance counter instance name for current process. This is truly strange ...");
        }

        public bool Equals(ProcessWraper other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return m_id.Equals(other.m_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProcessWraper) obj);
        }

        public override int GetHashCode()
        {
            return m_id.GetHashCode();
        }

        public static bool operator ==(ProcessWraper left, ProcessWraper right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProcessWraper left, ProcessWraper right)
        {
            return !Equals(left, right);
        }

        private void OnExited()
        {
            Exited?.Invoke(this, EventArgs.Empty);
        }

        private void FireStandartOutput(DataReceivedEventArgs e)
        {
            OnStandartOutput?.Invoke(this, e);
        }

        private void FireErrorOutput(DataReceivedEventArgs e)
        {
            OnErrorOutput?.Invoke(this, e);
        }

        #region IDispose Implementation

        private bool m_disposed;

        ~ProcessWraper()
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

                if (Process != null)
                {
                    Process.Exited -= ProcessOnExited;
                    Process.OutputDataReceived -= OnProcessOnOutputDataReceived;
                    Process.ErrorDataReceived -= OnProcessOnErrorDataReceived;
                    Process.Dispose();
                }

            }
            m_disposed = true;
        }

        #endregion

    }
}