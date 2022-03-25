#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ConsoleApplication2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var bla = new List<int>() {1, 2, 3, 4, 7, 8, 11, 15, 16, 17, 18};

            var hu = bla.OrderBy(d => d);
            
            var test = GetRanges(hu);


            Console.ReadLine();
        }

        public static List<Tuple<int, int>> GetRanges(IOrderedEnumerable<int> rows)
        {
            var list = new List<Tuple<int, int>>();
            var array = rows.ToArray();
            var start = array[0];
            var last = array[array.Length - 1];

            for (var i = 0; i < array.Length - 1; i++) 
            {
                var current = array[i];
                var next = array[i + 1];
                
                if (next == current + 1 && next != last) continue;
                
                list.Add(new Tuple<int, int>(start, next == last ? last : current));
                start = array[i + 1];
            }

            return list;
        }
    }
}