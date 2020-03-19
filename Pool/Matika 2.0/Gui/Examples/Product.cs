using System;

namespace Matika.Examples
{
    public class Product : Example
    {
        public Product(int difficulty)
        {
            int first;
            int second;
            var maximum = difficulty * 10 / 3;

            do
            {
                difficulty = difficulty == 10 ? difficulty - 1 : difficulty;

                first = new Random().Next(10 + 1);
                second = new Random().Next(difficulty + 1);

                Result = first * second;
            } while (Result < maximum);

            TaskString = string.Join(" ", first, Sign, second, EqualSign);
        }

        private static string Sign => " . ";
    }
}