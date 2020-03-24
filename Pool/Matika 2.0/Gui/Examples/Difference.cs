using System;

namespace Matika.Examples
{
    public class Difference : Example
    {
        public Difference(int diffuculty)
        {
            int maximum = diffuculty * 10;
            int first = new Random().Next(maximum + 1);
            int second = new Random().Next(10 + 1);

            Result = first - second;

            TaskString = string.Join(" ", first, Sign, second, EqualSign);
        }

        private static string Sign => " - ";
    }
}