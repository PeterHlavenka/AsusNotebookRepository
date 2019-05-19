using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderBy
{ // NA MULTISELECT COMBO BOXU STACI ZAPNOUT STARTSWITH = TRUE V XAMLU
    internal class Program
    {
        private static string Input = "N";

        private static void Main(string[] args)
        {      
            List<Person> persons = new List<Person>() { new Person(1, "N"), new Person(2, "N1"), new Person(3, "Tonda"), new Person(2, "1N"), new Person(2, "!N"),
                new Person(2, "N!"), new Person(2, "NN"), new Person(2, "N 101"), new Person(2, "N 103.4 HiTech RedLine"), new Person(2, "N 144G"),
                new Person(2, "N 113 HiTech"), new Person(2, "N!KTO tour 2015"), new Person(2, "n&sluno"), new Person(2, "N 6X2 NMX"), new Person(2, "N 97 MINI") };

            //Console.WriteLine("persons = persons.OrderBy(d => d.Name).ToList();");
            //persons = persons.OrderBy(d => d.Name).ToList();
            //Write(persons);
            //Console.WriteLine();

            //// vubec nefunguje pro order
            //Console.WriteLine("persons = persons.OrderBy(d => d.Name.StartsWith(Input)).ToList();");
            //persons = persons.OrderBy(d => d.Name.ToLower().StartsWith(Input.ToLower())).ToList();
            //Write(persons);
            //Console.WriteLine();

            //Console.WriteLine("persons = persons.OrderBy(d => d.Name.Length == Input.Length).ToList();");
            //persons = persons.OrderBy(d => d.Name.Length == Input.Length).ToList();
            //Write(persons);
            //Console.WriteLine();

            //// Tady je dulezite dat to na ToLower - je to CaseSensitive
            //Console.WriteLine("persons = persons.OrderBy(d => d.Name.IndexOf(Input)).ToList();");
            //persons = persons.OrderBy(d => d.Name.ToLower().IndexOf(Input.ToLower(), StringComparison.Ordinal)).ThenBy(x => x.Name.Length).ToList();
            //Write(persons);
            Console.WriteLine();

            //// Tady je dulezite dat to pripadne na ToLower - je to CaseSensitive
            //Console.WriteLine("persons = persons.Where(d => d.Name.Contains(Input)).OrderBy(d => d.Name.IndexOf(Input, StringComparison.Ordinal)).ThenBy(x => x.Name.Length).ToList();");
            //persons = persons.Where(d => d.Name.Contains(Input)).OrderBy(d => d.Name.IndexOf(Input, StringComparison.Ordinal)).ThenBy(x => x.Name.Length).ToList();
            //Write(persons);
            //Console.WriteLine();

            // Tady je dulezite dat to pripadne na ToLower - je to CaseSensitive, ToString returns Name
            Console.WriteLine("persons = persons.Where(d => d.Name.ToLower().Contains(Input.ToLower())).OrderBy(d => d.Name.ToLower().IndexOf(Input.ToLower(), StringComparison.Ordinal)).ThenBy(d => d.Name).ToList();");
            persons = persons.Where(d => d.Name.ToLower().Contains(Input.ToLower())).OrderBy(d => d.ToString().ToLower().IndexOf(Input.ToLower(), StringComparison.Ordinal)).ThenBy(d => d.ToString()).ToList();
            Write(persons);
            Console.WriteLine();

            Console.ReadLine();
        }

        public class Person
        {
            public Person(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public static void Write(List<Person> persons)
        {
            persons.ForEach(d => Console.WriteLine(d.Name));
        }
    }
}