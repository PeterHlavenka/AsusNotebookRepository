﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerazeniVlakenWaitPulse
{
    class Program
    {
        private static readonly object m_lock = new object ();
        private static volatile bool ready = false;

        static void Main (string[ ] args)
        {
            Task[ ] tasks = null;
            int counter = 0;
                    
            for (int i = 0 ; i < 100 ; i++)
            {
                counter = i; Console.WriteLine ("counter:" + counter);

                tasks = new[ ]
{
                Task.Factory.StartNew(() => DoSomething(1)),
                Task.Factory.StartNew(() => DoSomething(2)),
                Task.Factory.StartNew(() => DoSomething(3))
                 };

                
                while(!ready)
                {
                    Thread.Sleep (100);
                }

                lock (m_lock)
                {
                    Monitor.Pulse (m_lock);
                }

                Task.WaitAll (tasks);
                
            }

            Console.ReadLine ();
        }

        public static void DoSomething (int i)
        {           
            lock (m_lock)
            {
                if (i == 3)
                {
                    ready = true;
                    
                }
                Monitor.Wait (m_lock, Timeout.Infinite);   // vzda se rizeni a zaradi se do fronty
                Console.WriteLine (i);
                Monitor.Pulse (m_lock);
            }

            if (i == 3)
            {
                
                Console.WriteLine ();                
            }
           
        }
    }
}

