using System;

namespace Matika_2._0
{
    public class Addition : Example
    {
        private static string Sign => " + ";

        public Addition()
        {           
            var first = new Random().Next(Max);
            var second = new Random().Next(Max - first);

            Result = first + second;

            Task = string.Join(" ",first, Sign, second, EqualSign);            
        }
    }
}