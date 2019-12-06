using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanctellationTokenNaManualResetEventu
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manualResetEvent = new ManualResetEvent(false);

            // Rozbehnu task, ktery trva 15 sekund
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(15000); 
                manualResetEvent.Set();
            });

            // Vytvorim cancellation token
            var cancellationTokenSource = new CancellationTokenSource(); 

            // Vytvorim dalsi vlakno, ktere na cancellation tokenu zavola po dvou sekundach cancel()
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000); 
                cancellationTokenSource.Cancel();
            }, cancellationTokenSource.Token);


            // Zavola metodu
            NewMethod(manualResetEvent, cancellationTokenSource);
        }


        // Tato metoda ma cekat na resetEvent. Jenze dostala i cancellationToken, na kterem po dvou sekundach je zavolano cancel. Tim vyhodi vyjimku, odchytne ji a vypise na konzoli, ze bylo cancelnuto
        private static void NewMethod(ManualResetEvent manualResetEvent, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Task.Factory.StartNew(() => WaitHandle.WaitAll(new WaitHandle[] {manualResetEvent})).Wait(cancellationTokenSource.Token); // vyhodi operationCancelledException
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled");
                Console.ReadLine();
            }
        }
    }
}