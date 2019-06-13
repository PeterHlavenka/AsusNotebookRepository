using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanctellationTokenNaManualResetEventu
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationToken can1 = new CancellationToken();   // neni potreba - nevyuzito
            ManualResetEvent e = new ManualResetEvent(false);
            Task t1 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(15000);   // operace trva 15 sekund
                e.Set();
            }, can1);


            CancellationTokenSource cts = new CancellationTokenSource();  // hlavni a pouzity token
            Task tCancelling = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);  // ale po dvou sekundach je cancelnuta
                cts.Cancel();
            });

            NewMethod(e, cts);
        }

        private static void NewMethod(ManualResetEvent e, CancellationTokenSource cts)
        {
            try
            {
                Task.Factory.StartNew(() => WaitHandle.WaitAll(new[] { e }), cts.Token).Wait(cts.Token);   // vyhodi operationCancelledException
            }
            catch (OperationCanceledException)
            {

               Console.WriteLine("Operation was cancelled");
               Console.ReadLine();
            }
        }
    }
}
