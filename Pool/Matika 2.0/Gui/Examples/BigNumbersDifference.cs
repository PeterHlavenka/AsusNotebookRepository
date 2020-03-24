using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matika.Examples
{
    public class BigNumbersDifference : Example
    {
        public BigNumbersDifference(int diffuculty)
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