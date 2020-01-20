using System;
using System.Threading;

namespace SystemThreadingTimer
{
    internal class Program
    {
        // SYSTEM THREADING TIMER JE DISPOSABLE            MUSI SE DISPOSOVAT

        private static int Result { get; set; }

        private static Timer Timer { get; set; }

        private static void Main(string[] args)
        {
            Timer = new Timer(delegate // operace ktera se vykona v danem intervalu
                {
                    Result++;
                    Console.WriteLine(Result);

                    if (Result == 5)
                    {
                        StopTimer();
                        Console.WriteLine("Timer stopped");
                    }
                }
                , null // object is useful for providing the additional information required for the Timer operation. However, this State object is not mandatory and hence we can set it as null 
                , 0 // delay - o kolik se odlozi start timeru
                , 1000); // interval provadeni

            Console.ReadLine();
        }

        // Jak zastavit timer
        private static void StopTimer()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}