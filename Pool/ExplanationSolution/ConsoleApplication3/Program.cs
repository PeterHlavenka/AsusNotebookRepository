using System;

namespace ConsoleApplication3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Create an ordinary instance in the current AppDomain
            Worker localWorker = new Worker();
            localWorker.PrintDomain();

            // Create a new application domain, create an instance
            // of Worker in the application domain, and execute code
            // there.
            var wor = new Worker();
            var neco = typeof(Worker).Assembly.FullName;
            
            AppDomain ad = AppDomain.CreateDomain("New domain");
            var remoteWorker =  ad.CreateInstanceAndUnwrap(
                typeof(Worker).Assembly.FullName,
                "Worker");

            if (remoteWorker is Worker rw)
            {
                rw.PrintDomain();
            }
            

            Console.ReadLine();
        }
        

    }
    

}