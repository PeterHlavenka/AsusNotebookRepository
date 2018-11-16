using System;

namespace Matika_2._0
{
    public class Product : Example
    {
        private static string Sign => " * ";

        public Product()
        {
            int first;
            int second;

            do
            {
                first = new Random().Next(10 + 1);
                second = Max > 10 ?  new Random().Next((Max / 10) + 1) : new Random().Next(Max);

                Result = first * second;
            }
            while (Result > Max);

            Task = string.Join(" ", first, Sign, second, EqualSign);
        }        
    }
}