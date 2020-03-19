using System;
using System.Collections.Generic;
using System.Linq;
using Matika.Gui;
using Matika.Settings;

namespace Matika
{
    public class Conversion
    {
        public Conversion(IEnumerable<IConvertable> convertables)
        {
            Convertables = convertables;
        }

        public double Result { get; protected set; }
        public string TaskString { get; set; }
        protected static string EqualSign => " =  ";
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }

        public IEnumerable<IConvertable> Convertables { get; set; }

        public Conversion Generate(UnitConversionsSettingsViewModel settings)
        {
            var rand = new Random();
            var allowedConvertables = Convertables.First();  // delka zatim
            var dict = allowedConvertables.UnitsDictionary;
            var step = allowedConvertables.Step;
            var notNulls = dict.Where(d => d.Value != string.Empty).ToList();

            var number = new Random().Next(1, 1 + settings.Difficulty * settings.Difficulty * settings.Difficulty); 
            var stepDifference = settings.StepDifference;

            var from = notNulls.Skip(new Random().Next(notNulls.Count)).First();

            var highest = notNulls.Where(d => d.Key > from.Key).OrderBy(d => d.Key).ToList();
            var lowest = notNulls.Where(d => d.Key < from.Key).OrderByDescending(d => d.Key).ToList();

            var availables = highest.Take(stepDifference).ToList();
            availables.AddRange(lowest.Take(stepDifference));
           
            var to = availables.ElementAt(rand.Next(availables.Count));
               
            Result = from.Key < to.Key ? number / Math.Pow(step, (to.Key - from.Key)) : number * Math.Pow(step, (from.Key - to.Key));

            while (!settings.DecimalNumbers && Math.Abs(Result - (int)Result) > double.Epsilon)
            {
                number = number * 10;
                Result = from.Key < to.Key ? number / Math.Pow(step, (to.Key - from.Key)) : number * Math.Pow(step, (from.Key - to.Key));
            }

            FromUnit = from.Value;
            ToUnit = to.Value;
            TaskString = string.Join(" ", number, FromUnit, EqualSign);

            return this;               
        }
    }
}