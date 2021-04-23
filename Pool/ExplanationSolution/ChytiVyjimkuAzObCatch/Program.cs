using System;

namespace ChytiVyjimkuAzObCatch
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                try   // v prostrednim try neni catch, takze vyjimka jede dal.
                {

                    try
                    {
                        throw new Exception();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                finally
                {
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception jsem chytil az tady.  Mazec");
                
            }

            Console.ReadLine();
        }
    }
}