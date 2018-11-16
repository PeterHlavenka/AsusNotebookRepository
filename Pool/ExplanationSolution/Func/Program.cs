using System;

namespace Func
{
    internal class Program
    {
        //http://www.tutorialsteacher.com/csharp/csharp-func-delegate
        //metody jsou staticke protoze jde o konzolovou aplikaci a nemame objekt teto tridy

        private static void Main(string[] args)
        {
            //zavolame metodu a dame ji lambda vyraz ktery vrati double
            MetodaKteraNemaVstupAVratiDouble(() => 123.5);

            //misto lambdy muzeme pouzit metodu, je to uplne stejne. Lambda nahore je vlastne metoda bez vstupu vracejici double
            MetodaKteraNemaVstupAVratiDouble(VratDouble);


            MetodaKteraMaJedenVstupAVratiDouble(d => (double) d / 2);
            // je stejne jako tohle:          //jediny rozdil je, ze tady davame 10 jako parametr uz ted a v lambde ho predame az pri volani Invoke
            MetodaKteraMaJedenVstupAVratiDouble(d => VratDouble(10));


            MetodaKteraMaDvaVstupyAVratiDouble((a, b) => a + b);

            //Actions
            MetodaBezVstupuANavratu(() => Akce());
            MetodaBezVstupuANavratu(Akce);

            MetodaMaVstupANicNevraci(d => Akce(d));
            MetodaMaVstupANicNevraci(d => Akce(20));

            Console.ReadLine();
        }

        private static double VratDouble()
        {
            return 254.3;
        }

        private static double VratDouble(int value)
        {
            return (double) value / 2;
        }

        private static void Akce()
        {
            Console.WriteLine("Tato metoda nic nedostane ani nic nevrati");
            Console.WriteLine();
        }

        private static void Akce(int cislo)
        {
            Console.WriteLine($"Tato metoda dostala jako parametr: {cislo}");
            //Console.WriteLine();
        }

        private static void MetodaKteraNemaVstupAVratiDouble(Func<double> operace)
        {
            Console.WriteLine($"Vypise navratovou hodnotu operace: {operace.Invoke()}");
            Console.WriteLine();
        }

        private static void MetodaKteraMaJedenVstupAVratiDouble(Func<int, double> operace)
        {
            var hodnota = operace.Invoke(20);
            Console.WriteLine($"Vypise navratovou hodnotu operace, vstup dostane ve volani operace.Invoke(20): {hodnota}");


            Console.WriteLine($"Vypise navratovou hodnotu operace, vstup dostane ve volani operace.Invoke(5): {operace.Invoke(5)}");
            Console.WriteLine();
        }


        private static void MetodaKteraMaDvaVstupyAVratiDouble(Func<int, int, double> operace)
        {
            var hodnota = operace.Invoke(20, 30);
            Console.WriteLine($"Vypise navratovou hodnotu operace, vstupy dostane ve volani operace.Invoke(20, 30) a secte je: {hodnota}");
        }

        private static void MetodaBezVstupuANavratu(Action akce)
        {
            akce.Invoke();
        }

        private static void MetodaMaVstupANicNevraci(Action<int> akce)
        {
            akce.Invoke(20);
        }
    }
}