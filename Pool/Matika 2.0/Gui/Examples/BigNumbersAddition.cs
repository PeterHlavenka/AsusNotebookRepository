using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Matika.Settings;

namespace Matika.Examples
{
    public class BigNumbersAddition : Example
    {
        public BigNumbersAddition(BigNumbersSettingsViewModel settings)
        {
            int first;
            int second = 0;
            settings.WholeThousands = true;
            if (settings.WholeThousands)
            {
                first = new Random().Next(settings.FirstNumberSize) + 1;

                if (!settings.Overlaps)
                {
                    var firstNumberDigits = (int)Math.Floor(Math.Log10(first) + 1);
                    var secondNumberDigits = (int)Math.Floor(Math.Log10(settings.SecondNumberSize) + 1);
                    var skip = firstNumberDigits - secondNumberDigits;
                   
                    var list = new List<int>();
                    var test = first.ToString();
                    test = test.Substring(skip, test.Length - skip);

                    // pozpatku random cislo 
                    for (int i = test.Length; i > 0; i--)  //5
                    {

                        var sub = test.Substring(i - 1, 1);
                        var num = 9 - int.Parse(test.Substring(i -1, 1));
                        var ran = new Random().Next(num);
                        list.Add(ran);
                    }                                       

                    int counter = 1;
                    for (var i = 0; i < list.Count; i++)
                    {
                       var number =  list.ElementAt(i) * counter;
                       
                       second = second + number;
                       counter = counter * 10;
                    }

                    first = (first/1000)*1000;
                    second = (second/1000) * 1000;
                }
            }
            else
            {
                first = new Random().Next(settings.FirstNumberSize + 1);
                second = new Random().Next(settings.SecondNumberSize) + 1;
            }


            Result = first + second;

            TaskString = string.Join(" ", first.ToString("#,##0"), Sign, second.ToString("#,##0"), EqualSign);
        }

        private static string Sign => " + ";
    }
}