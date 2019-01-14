using System;
using System.Threading;

namespace HandShaking
{
    public static class EntryPoint
    {
        private static int m_counter;

        private static readonly object TheLock = new object();

        private static void ThreadFunc1()
        {
            lock (TheLock)
            {
                for (var i = 0; i < 50; ++i)
                {
                    Monitor.Wait(TheLock, Timeout.Infinite);
                    Console.WriteLine("{0} from Thread {1}",
                        ++m_counter,
                        Thread.CurrentThread.ManagedThreadId);
                    Monitor.Pulse(TheLock);
                }
            }
        }

        private static void ThreadFunc2()
        {
            lock (TheLock)
            {
                for (var i = 0; i < 50; ++i)
                {
                    Monitor.Pulse(TheLock);
                    Monitor.Wait(TheLock, Timeout.Infinite);
                    Console.WriteLine("{0} from Thread {1}",
                        ++m_counter,
                        Thread.CurrentThread.ManagedThreadId);
                }
            }
        }

        private static void Main()
        {
            var thread1 =
                new Thread(ThreadFunc1);
            var thread2 =
                new Thread(ThreadFunc2);
            thread1.Start();
            thread2.Start();

            Console.ReadLine();
        }
    }
}