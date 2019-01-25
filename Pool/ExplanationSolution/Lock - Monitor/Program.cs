using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lock___Monitor
{
    public static class Program
    {
        private static readonly object Lock = new object();
        private static decimal m_money = 130;
        private static AutoResetEvent m_are = new AutoResetEvent(false);
       

        private static void Main(string[] args)
        {
            Console.WriteLine("Program vytvori tri vlakna, ktere se pokusi vybrat 100 korun z uctu. Stav uctu na zacatku je 130,- \n" +
                              "Problem je, ze do kriticke sekce se za podminku  if (m_money > 100) dostanou vsechny tri vlakna, protoze v te chvili je jeste stav uctu opravdu 130 korun \n\n");

            Console.WriteLine("Priklad bez pouziti locku:\n");
           // Enumerable.Range(1, 3).Select(i => i).ToList().ForEach(i => Task.Factory.StartNew(() => { DoWork(i);}));

            var tasks = new[]
            {
                Task.Factory.StartNew(() => DoWork(1)),
                Task.Factory.StartNew(() => DoWork(2)),
                Task.Factory.StartNew(() => DoWork(3))
            };

            Task.WaitAll(tasks);

            Console.WriteLine("\n\nVratime stav uctu na 130,- a pouzijeme lock:\n");
            m_money = 130;
            tasks = new[]
            {
                Task.Factory.StartNew(() => DoWorkWithLock(1)),
                Task.Factory.StartNew(() => DoWorkWithLock(2)),
                Task.Factory.StartNew(() => DoWorkWithLock(3))
            };
            Task.WaitAll(tasks);
            

            Console.ReadLine();
        }

        private static void DoWork(int i)
        {
            if (m_money > 100)
            {
                
                Thread.Sleep(new Random().Next(500, 2000));
                Console.WriteLine($"    Vlakno "+i+" vybira 100");
                m_money = m_money - 100;
            }
            Console.WriteLine("    Stav uctu je: " + m_money);
        }

        private static void DoWorkWithLock(int i)
        {
            lock (Lock)
            {
                if (m_money > 100)
                {
                    Thread.Sleep(new Random().Next(500, 2000));
                    Console.WriteLine($"    Vlakno {0} vybira 100,", i);
                    m_money = m_money - 100;
                }               
            }
            Console.WriteLine("    Stav uctu je: " + m_money);
        }
    }
}