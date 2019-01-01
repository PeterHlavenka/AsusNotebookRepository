using System;

namespace Matika_2._0
{
    public class Addition : Example
    {
        public Addition(int difficulty)
        {
            do
            {
                var first = new Random().Next(difficulty + 1);
                var second = new Random().Next(difficulty) + 1;

                Result = first + second;
                Task = string.Join(" ", first, Sign, second, EqualSign);
            } while (Result < difficulty / 1.2);
        }

        private static string Sign => " + ";
    }
}