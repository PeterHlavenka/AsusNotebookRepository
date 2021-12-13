using System;

namespace DroidMatika
{
    public class ExampleBase
    {
        public decimal FirstNumber { get; set; }
        public decimal SecondNumber { get; set; }
        public decimal Result { get; set; }
        public string Operator { get; set; }

        protected decimal CreateNumber(int minValue, ParamsSource paramsSource)
        {
            int maxValue = paramsSource.Difficulty;
            bool decimalNumbers = paramsSource.DecimalNumbersAllowed;
            
            if (decimalNumbers)  // kdyz chci desetinna cisla vynasobim si parametry randomu
            {
                minValue *= 10;
                maxValue *= 10;
                
                var result = (decimal)
                 new Random().Next(minValue, maxValue + 1);    // a vysledny random vydelim deseti

                var test = result / 10;
                return result / 10;
            }
            
            return new Random().Next(minValue, maxValue + 1);
        }

        protected ParamsSource CreateParamsSource(ParamsSource origin, int difficulty)
        {
            return new ParamsSource()
            {
                DecimalNumbersAllowed = origin.DecimalNumbersAllowed,
                NegativeNumbersAllowed = origin.NegativeNumbersAllowed,
                Difficulty = difficulty
            };
        }

        protected ParamsSource CreateParamsSource(int difficulty, bool decimalNumbers, bool negativeNumbers)
        {
            return new ParamsSource()
            {
                DecimalNumbersAllowed = decimalNumbers,
                NegativeNumbersAllowed = negativeNumbers,
                Difficulty = difficulty
            };
        }
    }



    public class SumExample : ExampleBase
    {
        public SumExample(ParamsSource paramsSource)
        {
            FirstNumber = CreateNumber(1, paramsSource);
            SecondNumber = CreateNumber(1, paramsSource);
            Result = FirstNumber + SecondNumber;
            Operator = " + ";
        }
    }

    public class DiffExample : ExampleBase
    {
        public DiffExample(ParamsSource paramsSource)
        {
            var first = CreateNumber(1, paramsSource);
            var second = CreateNumber(1, paramsSource);

            if (paramsSource.NegativeNumbersAllowed)
            {
                FirstNumber = first;
                SecondNumber = second;
            }
            else
            {
                FirstNumber = Math.Max(first, second);
                SecondNumber = Math.Min(first, second);
            }

            Result = FirstNumber - SecondNumber;
            Operator = " - ";
        }
    }

    public class ProductExample : ExampleBase
    {
        public ProductExample(ParamsSource paramsSource)
        {
            FirstNumber = CreateNumber(1, paramsSource);
            SecondNumber = CreateNumber(0, CreateParamsSource(paramsSource, 10));
            Result = FirstNumber * SecondNumber;
            Operator = " x ";
        }
    }
    
    public class DivideExample : ExampleBase
    {
        public DivideExample(ParamsSource paramsSource)
        {
            var first = CreateNumber(1, paramsSource);

            var second = //paramsSource.DecimalNumbersAllowed ?  
                // chci desetinne cislo:
              //  CreateNumber(1, CreateParamsSource(paramsSource, 10)) : 
                // chci vzdy cele cislo
                CreateNumber(1, CreateParamsSource(paramsSource.Difficulty, false, paramsSource.NegativeNumbersAllowed));

            var result = first * second;

            // otoceni 
            FirstNumber = result;
            SecondNumber = second;
            Result = first;
            Operator = " : ";
        }
    }
}