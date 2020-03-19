using System;

namespace Matika.Examples
{
    public class BigNumbersAddition : Example
    {
        public BigNumbersAddition(int firsNumberDifficulty, int secondNumberDifficulty)
        {

                var first = new Random().Next(firsNumberDifficulty + 1)*1000;
                var second = (new Random().Next(secondNumberDifficulty) + 1)*1000;

                Result = first + second;               

                TaskString = string.Join(" ", first.ToString("#,##0"), Sign, second.ToString("#,##0"), EqualSign);            
        }

        private static string Sign => " + ";
    }
}
