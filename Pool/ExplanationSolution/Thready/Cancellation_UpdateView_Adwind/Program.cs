using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation_UpdateView_Adwind
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var frequencyAnalysis = new MyClassWithUpdateView();
            
            //Pustim dve vlakna
            for (var i = 0; i < 2; i++)
            {
                Thread.Sleep(1000);
                Task.Factory.StartNew(frequencyAnalysis.UpdateView);
            }

            Console.ReadLine();
        }
    }
}