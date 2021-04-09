using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaresearch.Framework.Utilities.Threading.TaskQueue;

namespace TaskQueueTest
{
    class Program
    {
        private static TaskQueue m_taskQueue;
        
        //  Vlozim do m_taskQueue tri tasky, vypisu cisla threadu pro main a pak pro thready jednotlivych tasku.
        // Tady je videt, ze se pouziva dokola jeden thread z poolu
        
        // DO TASK QUEUE LZE VLOZIT JEN ITASK, COZ JE DELEGATE TASK Z FRAMEWORKU
        static void Main(string[] args)
        {
            m_taskQueue = new TaskQueue();
            var isBusy = m_taskQueue.IsBusy;
            var maximalQueueLength = m_taskQueue.MaximalQueueLength;
            var neco = m_taskQueue.ClearQueueOnStopWork;
            
            // m_taskQueue.StopWork();
            // m_taskQueue.EnqueueTask(nejaky task);
            bool canEnqueueTask = m_taskQueue.CanEnqueueTask();
            
            Console.WriteLine($@"before adding task to queue, MainThread.ManagedThreadId = {Thread.CurrentThread.ManagedThreadId} ");


            for (int i = 0; i < 20; i++)
            {
                AddToQueue(i);
                Console.WriteLine($@"  task {i} added to queue");
            }
            
            Console.ReadLine();
        }

        public static void AddToQueue(int providedValue)
        {
            ITask loadDataTask = new DelegateTask($"LoadData(NormOrMessageId: {providedValue})", () =>
            {
                Console.WriteLine($@"Thread {Thread.CurrentThread.ManagedThreadId} go to sleep" );
                Thread.Sleep(3000);
                Console.WriteLine($@"Thread {Thread.CurrentThread.ManagedThreadId} waking up");
            });
            
            m_taskQueue.EnqueueTask(loadDataTask);
        }
    }
}