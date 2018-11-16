using System;

namespace Matika_2._0
{
    public class Share : Example
    {
        private static string Sign => " : ";

        public Share()
        {
            int fir;
            int sec;
            int res;

            do
            {
                fir = new Random().Next(10 + 1);
                sec = Max > 10 ? new Random().Next(1, (Max / 10) + 1) : new Random().Next(1, Max);

                res = fir * sec;
            }
            while (res > Max);

            Result = fir;

            Task = string.Join(" ", res, Sign, sec, EqualSign);
        }
    }
}