using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenNaAsyncAwaitu
{
    
    
    // NOT WORKING
    internal class Program : IDisposable
    {
        // ReSharper disable once InconsistentNaming
        private static CancellationTokenSource m_lastCancellationTokenSource = new CancellationTokenSource();
        private static readonly object m_lock = new object();


        public void Dispose()
        {
            lock (m_lock)
            {
                m_lastCancellationTokenSource?.Cancel();
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(@"Start");

            //LongFunctionWithCancellationException(@"prvni"); // 1)


            for (int i = 0; i < 20; i++)
            {
                LongFunctionWithCancellationRequestedCheck(i.ToString());
            }

            Console.ReadLine();
        }


        private static async void LongFunctionWithCancellationException(string message)
        {
            lock (m_lock)
            {
                m_lastCancellationTokenSource?.Cancel();
                m_lastCancellationTokenSource = new CancellationTokenSource();
            }

            await Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(3000);

                    // 1) muzu bud vyhodit vyjimku (tu bych ale musel chytat v Catch(OperationCancelledException) nebo jinem catchi)
                    m_lastCancellationTokenSource.Token.ThrowIfCancellationRequested();

                    Console.WriteLine($@"Metoda 1 s vyjimkou : Pokud bude v tech trech sekundach zavolano cancell na m_lastCancellationTokenu, pak se to sem nikdy nedostane. {message}");
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine();
                    Console.WriteLine(@"Odchycena cancellation vyjimka, cancellnuto bylo na radku 18");
                }
            }, m_lastCancellationTokenSource.Token).ConfigureAwait(true);
        }


        private static void LongFunctionWithCancellationRequestedCheck(string message)
        {
            lock (m_lock)
            {
                m_lastCancellationTokenSource?.Cancel();
               // m_lastCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_lastCancellationTokenSource.Token);

                Console.WriteLine(@"2) Vytvoril jsem novy cancellationToken a predam ho novemu threadu. ");


               
                    Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(3000);

                        Console.WriteLine(message);
                    }, m_lastCancellationTokenSource.Token);
            }
        }
    }
}