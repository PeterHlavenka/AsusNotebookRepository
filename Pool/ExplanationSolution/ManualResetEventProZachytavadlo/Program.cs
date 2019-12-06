using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventProZachytavadlo
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            // Byl byhozen ActualStreamChanged, vznikaji dve vlakna. Jedno jde pres Estimatora, druhe pres ThumbnailsProvidera (trva dele).
            // Chceme, aby do FileStreamStorage dorazilo nejprve to vlakno od Providera.

            Task.Factory.StartNew(() =>
            {
                // Vlakno Estimatora ceka na signal od providera, ze muze bezet;

                manualResetEvent.WaitOne();

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("                     Estimator vlakno bezi");
                    Thread.Sleep(1000);
                }
            });

            Task.Factory.StartNew(() =>
            {
                // Vlakno providera dela nejakou praci, po nekolika sekundach posle signal, ze muze zacit estimator

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Provider vlakno bezi");
                    Thread.Sleep(1000);

                    if (i == 3)
                    {
                        Console.WriteLine("   Provider vlakno ma hotovo");
                        manualResetEvent.Set();
                    }
                }
            });

            Console.ReadLine();
        }
    }
}
