using System;

namespace Matika.Examples
{
    public class Share : Example
    {
        private static string Sign => " : ";

        public Share(int difficulty)
        {
            int res = new Random().Next(10 + 1);
            int sec = new Random().Next(1, difficulty + 1);
            int fir = res * sec;


            Result = res;

            TaskString = string.Join(" ", fir, Sign, sec, EqualSign);
        }
    }
}