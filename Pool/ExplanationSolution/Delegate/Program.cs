using System;

namespace Delegate
{
    internal class Program
    {
        // delegat je reference na metodu
        private static void Main(string[] args)
        {
            //deklarace
            MathOperation.Delegate del1, del2;

            //inicializace -> rekneme ze del1 ukazuje na metodu Soucet
            del1 = MathOperation.Soucet;

            //pouziti. V podstate misto Soucet(5, 2)  napiseme  del1(5, 2)  jen vymenime skutecny nazev metody za zastupce
            Console.WriteLine(del1(5, 2));


            del2 = MathOperation.Rozdil;

            Console.WriteLine(del2(5, 2));

            Console.ReadLine();
        }

        public void VypisNejakouMatematickouOperaci(Func<double> operace)
        {
            Console.WriteLine(operace);
        }

        private class MathOperation
        {
            //Modifikatorpristupu, KlicoveSlovo:delegate, NavratovyTyp, Nazev (formalni parametry)
            public delegate double Delegate(int a, int b);

            //metody ktere se trefuji do predpisu delegata
            public static double Soucet(int a, int b)
            {
                return a + b;
            }

            public static double Rozdil(int a, int b)
            {
                return a - b;
            }
        }
    }
}