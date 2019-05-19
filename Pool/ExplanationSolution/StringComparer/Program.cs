using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace StringComparer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<string> words = new List<string> {"N", "!Neco", "!!Heel", "Heel", "! N 101", "BOmba"};
            List<Person> persons = new List<Person>() {new Person(1, "Petr"), new Person(2, "Michal"), new Person(3, "Tonda")}; 

            
            // 1) Pokud mam tridu, ktera dedi od IComparer<T> a umi porovnat objekty muzu ji pouzit takto:   (bue pouzito porovnavani v teto tride:)
            var comparer = new CaseSensitiveComparer();
            words.Sort(comparer);

            // 2) Osoby muzu radit podle kriteria, ktere jim dam do lambdy:
           // persons.Sort((Person x, Person y) => {x.CompareTo(y)}); nejak tak..


            // Sort metoda musi vedet jak ma prvky seradit. Pomoci lambdy vytvorim comparer, ktery predam metode Sort a ta bude radit podle nej.
            // Metoda CompareTo je schopna porovnat vsechny tridy, ktere implementuji rozhrani IComparable. Napr. Integer.
            words.Sort((x, y) =>
            {
                if (Char.IsLetterOrDigit(x[0]))
                {
                    if (!Char.IsLetterOrDigit(y[0]))
                    {
                        // x is a letter/digit and y is not, override regular CompareTo
                        return -1;
                    }
                }
                else if (Char.IsLetterOrDigit(y[0]))
                {
                    // y is a letter/digit and x is not, override regular CompareTo
                    return 1;
                }
                return x.CompareTo(y);
            });

            // Protoze jde o stringy a ty jsou comparable, muzu udelat toto:
            words.Sort();

            // pokud by slo o objekty
            words.Sort((x, y) => x.CompareTo(y));


            foreach (var s in words)
            {
                Console.WriteLine(s + "\n");
            }


            Console.ReadLine();
        }
    }

    // 1) Muzu mit tridu, ktera mi porovnava objekty. 
    public class CaseSensitiveComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }

    // 2) Komplexni typ - metoda Sort nevi podle ceho ma porovnat - podle Id nebo podle Name?
    public class Person: IComparable<Person>
    {
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }


      // Prvni moznost je, ze by trida implementovala IComparable a v metode CompareTo by rekla, jak se tyto typy radi:
      public int CompareTo(Person other)
      {
         return this.Name.CompareTo(other.Name); // porovnavej podle jmena
         // return this.Id.CompareTo(other.Id);  // porovnavej podle Id
      }
    }
}