using System;

namespace Delegate2
{
    internal class Program
    {
        public delegate void PozdravDelegate(string jmeno);

        private static void Main(string[] args)
        {
            PozdravDelegate del1, del2, del3, del4;

            //timto rekneme, ze delegat del1 ukazuje na metodu Ahoj
            del1 = Ahoj;
            del2 = Nashledanou;


            //slozeni delegatu
            del3 = del1 + del2;
            del3("Petr");   //zavola metodu Ahoj a Nashledanou
            Console.WriteLine();

            //dekomponovani delegatu
            del4 = del3 - del1;
            del4("Petr");

            Console.ReadLine();
        }

        public static void Ahoj(string jmeno)
        {
            Console.WriteLine("Zdravime " + jmeno);
        }

        public static void Nashledanou(string jmeno)
        {
            Console.WriteLine("Nashledanou " + jmeno);
        }
    }
}

