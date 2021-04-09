using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThrowIfCancellationRequestedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100000000; i++)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    Console.WriteLine(i);
                }
            }, tokenSource.Token);
            
            Thread.Sleep(2000);
            tokenSource.Cancel();

            Console.ReadLine();
        }
    }
}