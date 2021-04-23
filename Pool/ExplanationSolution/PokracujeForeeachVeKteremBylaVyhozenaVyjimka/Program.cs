using System;

namespace PokracujeForeeachVeKteremBylaVyhozenaVyjimka
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);

                    if (i == 3)
                    {
                        throw new Exception();
                    }
                
                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}