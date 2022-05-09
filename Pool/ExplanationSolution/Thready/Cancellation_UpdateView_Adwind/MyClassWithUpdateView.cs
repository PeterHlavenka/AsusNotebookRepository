using System;
using System.Threading;

namespace Cancellation_UpdateView_Adwind
{
    public class MyClassWithUpdateView
    {
        private static CancellationTokenSource _lastCancellationTokenSource;

        public void UpdateView()
        {
            _lastCancellationTokenSource?.Cancel();
            var cancellationTokenSource = new CancellationTokenSource(); // kazde vlakno vytvori novy tokenSource 
            _lastCancellationTokenSource = cancellationTokenSource; // referenci na nej si ulozi do _last

            Method_A();

            if (cancellationTokenSource.IsCancellationRequested) 
            {
                Console.WriteLine("Vlakno " + Thread.CurrentThread.ManagedThreadId + " bylo zastaveno a nebude pokracovat do metody B");
                return;
            }

            Method_B();
        }

        private void Method_A()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine(@"Method_A: " + i + ". Thread " + threadId);
            }
        }

        private void Method_B()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            for (var i = 10; i < 15; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine(@"Method_B: " + i + ". Thread " + threadId);
            }
        }
    }
}