using System;

namespace Matika.Examples
{
    public class Addition : Example
    {
        public Addition(int difficulty)
        {
            do
            {
                int first = new Random().Next(difficulty + 1);
                int second = new Random().Next(difficulty) + 1;

                Result = first + second;
                TaskString = string.Join(" ", first, Sign, second, EqualSign);
            } while (Result < difficulty / 1.2);
        }

        private static string Sign => " + ";
    }
}