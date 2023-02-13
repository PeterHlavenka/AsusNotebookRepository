using System;
using System.Threading.Tasks;
using LibraryAsDll_48;

namespace Wind.Login
{
    /// <summary>
    /// Hlavní doména potřebuje nějak vědět, že hlavní thread druhé domény už skončil...
    /// </summary>



    public class MarshalByRefKeepAliveObject : MarshalByRefObject
    {
        public sealed override object InitializeLifetimeService()
        {
            // bez tohoto by po 5 minutách mezidoménové nečinosti došlo k uvolnění objektu garbage-collectorem.
            return null;
        }
    }

    
    public class Isolated<T> : IDisposable where T : MarshalByRefObject, IAsyncShutdown
    {
        // private static readonly ILog s_log = Log4NetUtils.GetCurrentClassLogger();

        private readonly string m_name;
        private AppDomain m_domain;
        private ShutdownInitiatorAndMonitor m_shutdownMonitor;
        private T m_object;


        public Isolated(string name = null)
        {
            m_name = name ?? "Isolated " + typeof(T).Name;
            
            var thisSetup = AppDomain.CurrentDomain.SetupInformation;
            var setup = new AppDomainSetup
                {
                    ApplicationBase = thisSetup.ApplicationBase,
                    ApplicationName = thisSetup.ApplicationName,
                    LoaderOptimization = thisSetup.LoaderOptimization
                };
            
            m_domain = AppDomain.CreateDomain(m_name, AppDomain.CurrentDomain.Evidence, setup);

            // s_log.Debug(Enumerate.Items("Created AppDomain " + m_name + " with:",
            //                             "setup.ApplicationBase =    " + setup.ApplicationBase,
            //                             "setup.ApplicationName =    " + setup.ApplicationName,
            //                             "setup.LoaderOptimization = " + setup.LoaderOptimization).JoinLines());

            Type type = typeof(T);
            m_object = (T)m_domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
            m_shutdownMonitor = new ShutdownInitiatorAndMonitor(m_object);
        }


        public T Object
        {
            get
            {
                if (!IsAlive)
                    throw new InvalidOperationException("Secondary AppDomain is closing.");

                return m_object;
            }
        }

        public bool IsAlive
        {
            get { return !m_shutdownMonitor.ShutdownInitiated; }
        }

        public void Dispose()
        {
            if (m_domain == null)
                return;

            if (m_shutdownMonitor != null)
            {
                m_shutdownMonitor.InitiateShutdown();
                m_shutdownMonitor.Wait();
                m_shutdownMonitor = null;
            }

            AppDomain.Unload(m_domain);
            m_object = null;
            m_domain = null;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}",
                                 m_name, IsAlive
                                     ? "running"
                                     : m_domain == null ? "unloaded" : "shutdown started");
        }

        
        class ShutdownInitiatorAndMonitor : MarshalByRefKeepAliveObject
        {
            private readonly IAsyncShutdown m_slave;
            private TaskCompletionSource<bool> m_shutdown;

            public ShutdownInitiatorAndMonitor(IAsyncShutdown slave)
            {
                m_slave = slave;
                m_slave.HasShutdown += OnShutdown;
            }


            public bool ShutdownInitiated
            {
                get { return m_shutdown != null; }
            }

            public void InitiateShutdown()
            {
                lock (this)
                {
                    if (m_shutdown == null)
                    {
                        m_shutdown = new TaskCompletionSource<bool>();
                        m_slave.BeginShutdown();
                    }
                }
            }

            public void Wait()
            {
                m_shutdown.Task.Wait();
            }

            private void OnShutdown(object sender, EventArgs eventArgs)
            {
                lock (this)
                {
                    if (m_shutdown == null)  // shutdown bez requestu
                        m_shutdown = new TaskCompletionSource<bool>();

                    m_shutdown.SetResult(true);
                }
            }
        }
    }
}
