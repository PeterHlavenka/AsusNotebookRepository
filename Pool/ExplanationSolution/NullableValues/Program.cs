using System;

namespace NullableValues
{
    internal class Program
    {
        private static int test1;
        private static bool? MyBool { get; set; }
        private static bool Result { get; set; }

        private static void Main(string[] args)
        {
            int? test = null;

            if (test.HasValue)
            {
                test1 = test.Value;
            }

            MetodaCoBereInt(test1);

            var necoTest = new Neco();

            MyBool = null;
            Neco neco = null;

            var testHodnoty = MyBool.HasValue && MyBool.Value; // BUDE FALSE
            var testObjektu = neco == null; // OBJEKTY NEMAJI METODU HAS VALUE

            // VYHODI EXCEPTION - NULLABLE OBJECT MUST HAVE VALUE;
            Result = MyBool.Value;

            Console.WriteLine(Result);
            Console.ReadLine();
        }

        private static void MetodaCoBereInt(int value)
        {
            var aa = value;
        }
    }

    internal class Neco
    {
        public Neco(int? value = null)
        {
            var te = value.Value; // POKUD NECHECKNU .HAS VALUE , DOSTANU VYJIMKU
        }
    }
}