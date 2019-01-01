using System;

namespace Matika_2._0
{
    public class Difference : Example
    {
        public Difference(int diffuculty)
        {
            var maximum = diffuculty * 10;
            var first = new Random().Next(maximum + 1);
            var second = new Random().Next(10 + 1);

            Result = first - second;

            Task = string.Join(" ", first, Sign, second, EqualSign);
        }

        private static string Sign => " - ";
    }
}