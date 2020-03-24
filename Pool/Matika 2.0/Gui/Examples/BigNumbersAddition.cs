using System;
using System.Collections.Generic;
using System.Linq;
using Matika.Settings;

namespace Matika.Examples
{
    public class BigNumbersAddition : Example
    {
        public BigNumbersAddition(BigNumbersSettingsViewModel settings)
        {
            var second = 0;
            var first = new Random().Next(settings.FirstNumberSize) + 1;
            try
            {
                if (!settings.Overlaps) // zakazane presahy
                {
                    var firstNumberDigits = (int) Math.Floor(Math.Log10(first) + 1);
                    var secondNumberDigits = (int) Math.Floor(Math.Log10(settings.SecondNumberSize) + 1);
                    if (firstNumberDigits > secondNumberDigits)
                    {
                        secondNumberDigits = firstNumberDigits;
                    }

                    var skip = firstNumberDigits - secondNumberDigits;

                    var list = new List<int>();
                    var test = first.ToString();
                    test = test.Substring(skip, test.Length - skip);

                    for (var i = test.Length; i > 0; i--)
                    {
                        var sub = test.Substring(i - 1, 1);
                        var num = 9 - int.Parse(test.Substring(i - 1, 1));
                        var ran = new Random().Next(num);
                        list.Add(ran);
                    }

                    var counter = 1;
                    for (var i = 0; i < list.Count; i++)
                    {
                        var number = list.ElementAt(i) * counter;

                        second = second + number;
                        counter = counter * 10;
                    }
                }
                else // povolene presahy
                {
                    second = new Random().Next(settings.SecondNumberSize) + 1;
                }

                // chceme jen cele tisicovky
                if (settings.WholeThousands)
                {
                    first = first / 1000 * 1000;
                    second = second / 1000 * 1000;
                }
            }
            catch (Exception e)
            {
                first = 1;
                second = 1;
            }



            Result = first + second;

            TaskString = string.Join(" ", first.ToString("#,##0"), Sign, second.ToString("#,##0"), EqualSign);
        }

        private static string Sign => " + ";
    }
}