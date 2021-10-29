using System;

namespace DroidMatika
{
    public class ExampleBase
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public decimal Result { get; set; }
        public string Operator { get; set; }

        protected int CreateNumber(int minValue, int maxValue)
        {
            return new Random().Next(minValue, maxValue + 1);
        }
    }



    public class SumExample : ExampleBase
    {
        public SumExample(int difficulty)
        {
            FirstNumber = CreateNumber(1, difficulty);
            SecondNumber = CreateNumber(1, difficulty);
            Result = FirstNumber + SecondNumber;
            Operator = " + ";
        }
    }

    public class DiffExample : ExampleBase
    {
        public DiffExample(int difficulty)
        {
            var first = CreateNumber(1, difficulty);
            var second = CreateNumber(1, difficulty);

            FirstNumber = Math.Max(first, second);
            SecondNumber = Math.Min(first, second);
            Result = FirstNumber - SecondNumber;
            Operator = " - ";
        }
    }

    public class ProductExample : ExampleBase
    {
        public ProductExample(int difficulty)
        {
            FirstNumber = CreateNumber(1, difficulty);
            SecondNumber = CreateNumber(0, 10);
            Result = FirstNumber * SecondNumber;
            Operator = " . ";
        }
    }
    
    public class DivideExample : ExampleBase
    {
        public DivideExample(int difficulty)
        {
            var first = CreateNumber(1, difficulty);
            var second = CreateNumber(1, 10);
            var result = first * second;

            // otoceni at tam nejsou desetinna mista
            FirstNumber = result;
            SecondNumber = first;
            Result = second;
            Operator = " : ";
        }
    }
}