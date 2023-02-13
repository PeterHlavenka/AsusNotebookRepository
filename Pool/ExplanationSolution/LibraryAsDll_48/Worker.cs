using System;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using Caliburn.Micro;
using PricingWindow48;

namespace LibraryAsDll_48
{
    public class Worker : MarshalByRefObject, IAsyncShutdown
    {
        private Thread m_uiThread;
        private PricingWindow m_window;

        public void PrintDomain()
        {
            Console.WriteLine("Object is executing in AppDomain \"{0}\"",
                AppDomain.CurrentDomain.FriendlyName);
        }

        public event EventHandler HasShutdown;
        public void BeginShutdown()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Metoda jen nastartuje UI thread, a okamžitě se vrátí. Property Model čeká, dokud neproběhne 
        /// komplet inicializace. 
        /// </summary>
        /// <param name="service">něco, co se umí přihlásit k serveru, a žije to v jiné AppDomain</param>
        /// <param name="clientConfig">OBSAH Kite configu, odkud se přečte adresa serveru atd.</param>
        /// <param name="workingDirectory">adresář, kam se ukládá naposledy použité username/password a logy</param>
        public void Start()
        {
            Thread newThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            
            
            // nastartuje Login_UI thread
            // m_uiThread = new Thread(RunWindow);
            // m_uiThread.SetApartmentState(ApartmentState.STA);
            // m_uiThread.IsBackground = false;
            // m_uiThread.Name = "Login_UI";
            // m_uiThread.Start();

            Console.WriteLine("UI thread started.");
            Console.WriteLine();
        }
        
        static void ThreadStartingPoint()
        {
            PricingWindow pricingWindow = new PricingWindow();
            pricingWindow.ShowInTaskbar = true;
            pricingWindow.Show();
            Dispatcher.Run(); 
        }

        private void RunWindow()
        {
            try
            {
               //InitializeUiThread();
                var model = new PricingWindowViewModel();

                // if (m_showWindow)
                // {
                    m_window = new PricingWindow();
                    // m_window.LoginWindowLoaded += ReadyToStart;
                    // m_window.Closed += (sender, args) => m_dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                    Execute.BeginOnUIThread(m_window.Show);
                // }
                // else
                // {
                //     Execute.BeginOnUIThread(ReadyToStart);
                // }

                // odblokuje property Model, a tím pádem umožní přístup němu z jiných threadů.
               // m_loginModelTask.SetResult(model);

                Dispatcher.Run();
                // End();
            }
            catch (Exception ex)
            {
                // End(ex);
                // m_log.Error(ex);
                throw new Exception();
            }
        }
    }
}