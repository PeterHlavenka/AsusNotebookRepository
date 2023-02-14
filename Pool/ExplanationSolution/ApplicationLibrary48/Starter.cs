using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Threading;

namespace ApplicationLibrary48
{
    public class Starter
    {
        public void Start()
        {
            LaunchDotNet48WpfApplication();

            // Does not work in .net6
            // Thread newThread = new Thread(new ThreadStart(ThreadStartingPoint));
            // newThread.SetApartmentState(ApartmentState.STA);
            // newThread.Start();
            // Console.WriteLine("UI thread started.");
            // Console.WriteLine();
        }
        
        // napssat application ktera se bude sopoustet pomoci 
        private void LaunchDotNet48WpfApplication()
        {
            string pathToDotNet48WpfApplication = @"d:\AsusNotebookRepository\Pool\ExplanationSolution\WpfApplication48\bin\Debug\WpfApplication48.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(pathToDotNet48WpfApplication);
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            Process.Start(startInfo);
        }


        // obalit to spousteci a zastavovaci servicou projekt Host_HostedService_StartStop
        // komunikovat pomoci grpc
        
        static void ThreadStartingPoint()
        {
            // Does not work in .net6
            Pricing48Window pricingWindow = new Pricing48Window();
            pricingWindow.ShowInTaskbar = true;
            pricingWindow.Show();
            Dispatcher.Run(); 
        }

    }
}