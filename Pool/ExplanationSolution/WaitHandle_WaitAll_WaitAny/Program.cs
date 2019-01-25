using System;
using System.Threading;

namespace WaitHandle_WaitAll_WaitAny
{
    public sealed class App
    {
        // Define an array with two AutoResetEvent WaitHandles.
        private static readonly WaitHandle[] WaitHandles =
        {
            new AutoResetEvent(false),
            new AutoResetEvent(false)
        };

        // Define a random number generator for testing.
        private static readonly Random r = new Random();

        private static void Main()
        {
            DisplayInfo();

            // Queue up two tasks on two different threads; 
            // wait until all tasks are completed.
            var dt = DateTime.Now;
            Console.WriteLine("Main thread is waiting for BOTH tasks to complete.");
            ThreadPool.QueueUserWorkItem(DoTask, WaitHandles[0]);
            ThreadPool.QueueUserWorkItem(DoTask, WaitHandles[1]);
            WaitHandle.WaitAll(WaitHandles);
            // The time shown below should match the longest task.
            Console.WriteLine("Both tasks are completed (time waited={0})", (DateTime.Now - dt).TotalMilliseconds);
                

            // Queue up two tasks on two different threads; 
            // wait until any tasks are completed.
            dt = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("The main thread is waiting for either task to complete.");
            ThreadPool.QueueUserWorkItem(DoTask, WaitHandles[0]);
            ThreadPool.QueueUserWorkItem(DoTask, WaitHandles[1]);
            var index = WaitHandle.WaitAny(WaitHandles);
            // The time shown below should match the shortest task.
            Console.WriteLine("Task {0} finished first (time waited={1}).", index , (DateTime.Now - dt).TotalMilliseconds);
                

            Console.ReadLine();
        }

        private static void DisplayInfo()
        {
            Console.WriteLine
                ("Aplikace vytvori dva AutoResetEventy. To jsou objekty, ktere dedi od tridy WaitHandle.\n" +
                 "Na vytvoreni threadu pouzije ThreadPool a jeho metodu ThreadPool.QueueUserWorkItem(DoTask, WaitHandles[0]);\n" +
                 "ktere preda waitCallBack - coz je metoda z main threadu, pomoci ktere muze vedlejsi thread komunikovat s main threadem."
                );

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DoTask(object state)
        {
            var are = (AutoResetEvent) state;
            var time = 1000 * r.Next(2, 10);
            Console.WriteLine("Performing a task for {0} milliseconds.", time);
            Thread.Sleep(time);
            are.Set();
        }
    }

    // This code produces output similar to the following:
    //
    //  Main thread is waiting for BOTH tasks to complete.
    //  Performing a task for 7000 milliseconds.
    //  Performing a task for 4000 milliseconds.
    //  Both tasks are completed (time waited=7064.8052)
    // 
    //  The main thread is waiting for either task to complete.
    //  Performing a task for 2000 milliseconds.
    //  Performing a task for 2000 milliseconds.
    //  Task 1 finished first (time waited=2000.6528).
}