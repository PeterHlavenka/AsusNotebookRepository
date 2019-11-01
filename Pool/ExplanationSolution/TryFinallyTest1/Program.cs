using System;

namespace TryFinallyTest1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                try
                {
                    var test = 10;
                    var result = test / 0;
                }
                finally
                {
                    Console.WriteLine("vyjimka pri deleni nulou");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Vyjimka se zpropaguje i bez bloku catch 

{e}");
                throw;
            }
        }
    }
}