using System;

namespace Matika_2._0
{
    public class Example
    {
        public int Result { get; protected set; }       
        public string Task { get; set; }
        protected static string EqualSign => " = ";
        protected static int Max { get; private set; }


        public Example Generate(int max)
        {
            Max = max;

            //var rand = new Random().Next(3);
            var rand = 3;

            switch (rand)
            {
                case 0:
                    return new Addition();
                case 1:
                    return new Difference();
                case 2:
                    return new Product();
                case 3:
                    return new Share();
                default:
                    throw new ArgumentOutOfRangeException(nameof(rand));
            }            
        }
    }
}