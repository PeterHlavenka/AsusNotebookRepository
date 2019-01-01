using System;

namespace Matika
{
    public class Share : Example
    {
        private static string Sign => " : ";

        public Share(int difficulty)
        {
            var res = new Random().Next(10 + 1);
            var sec = new Random().Next(1, difficulty + 1);
            var fir = res * sec;


            Result = res;

            Task = string.Join(" ", fir, Sign, sec, EqualSign);
        }
    }
}