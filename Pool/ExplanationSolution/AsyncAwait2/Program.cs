using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwait2
{
    class Program
    {
        static void  Main(string[] args)
        {
            Console.WriteLine("volam Run");
            Run();

            Console.ReadLine();
        }

        private static async  Task<int> VratPocetZnakuAsync()
        {
            HttpClient client = new HttpClient();

            Task<string> ulohaSObsahemWebu = client.GetStringAsync("http://www.seznam.cz");
            Console.WriteLine("Zacinam stahovat\n");
            string obsah = await (ulohaSObsahemWebu);

            Console.WriteLine($"Pocet znaku {obsah.Length}");

            // úloha běží, zde můžeme spouštět další nezávislý kód
            // ...
            Console.WriteLine("kod po spusteni stahovani jede dal nezastavil se a neceka na dokonceni ulohySObsahemWebu");

            // předání kontroly programu metodě, která nás zavolala, dokud se úloha nedokončí.
            // await z ní následně zároveň získá výsledek
            

            //vracime bezici ulohu podle navratoveho typu, ktera ale po skonceni cinnosti vrati tento int:
            return obsah.Length;  
        }

        private static async void Run()
        {
            

            int pocetZnaku = await VratPocetZnakuAsync();
           
        }
    }
}
