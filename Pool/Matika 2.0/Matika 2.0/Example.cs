using System;
using System.Collections.Generic;

namespace Matika_2._0
{
    public class Example
    {
        public int Result { get; protected set; }
        public string Task { get; set; }
        protected static string EqualSign => " =  ";
        private static int Diff { get; set; }


        public Example Generate(SettingsDialogViewModel settings)
        {
            Diff = settings.Difficulty;

            var list = new List<int>();

            for (var i = 0; i < settings.AddCount; i++)
            {
                list.Add(0);
            }
            for (var i = 0; i < settings.DifferenceCount; i++)
            {
                list.Add(1);
            }
            for (var i = 0; i < settings.ProductCount; i++)
            {
                list.Add(2);
            }
            for (var i = 0; i < settings.DivideCount; i++)
            {
                list.Add(3);
            }

            var rand = 0;

            if (list.Count > 0)
            {
                var r = new Random().Next(list.Count);
                rand = list[r];
            }


            switch (rand)
            {
                case 0:
                    return new Addition(Diff);
                case 1:
                    return new Difference(Diff);
                case 2:
                    return new Product(Diff);
                case 3:
                    return new Share(Diff);
                default:
                    throw new ArgumentOutOfRangeException(nameof(rand));
            }
        }
    }
}