using System;
using System.Collections.Generic;
using ConcurrentCollections;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            var jedna = new HashSet<int>() {1, 2};
            var dva = new HashSet<int>() {1, 2, 3};

            var test = jedna.SetEquals(dva);

            
            var tri = new ConcurrentHashSet<int>() {1, 2};
            var ctyri = new ConcurrentHashSet<int>(1,2);

            var test2 = tri.Equals(ctyri);
            var test3 = tri.Equals(ctyri);

            var list = new List<int>() {1, 2};
            var lidva = new List<int>() {1, 2};


            var test4 = lidva.Equals(list);
           

            Console.ReadLine();
        }
    }
}