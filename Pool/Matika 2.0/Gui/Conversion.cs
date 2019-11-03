using System;
using System.Collections.Generic;
using System.Linq;
using Matika.Gui;

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
            var allowedConvertables = Convertables.First();
            var dict = allowedConvertables.UnitsDictionary;
            var step = allowedConvertables.Step;

            

            var number = new Random().Next(1, 1 + settings.Difficulty * settings.Difficulty); 
           
            var notNulls = dict.Where(d => d.Value != string.Empty).ToList();

            var stepDifference = Math.Min(settings.Difficulty, notNulls.Count);

            KeyValuePair<int, string> from = notNulls.Skip(new Random().Next(notNulls.Count)).First();
            KeyValuePair<int, string> to;

            var highest = notNulls.Where(d => d.Key > from.Key).OrderBy(d => d.Key).ToList();
            var lowest = notNulls.Where(d => d.Key <= from.Key).OrderByDescending(d => d.Key).ToList();

            if (highest.Count > lowest.Count)
            {                
                to = highest.Skip(rand.Next(Math.Min(highest.Count -1, stepDifference))).First();
            }
            else
            {
                 to = lowest.Skip(rand.Next(1, Math.Min(lowest.Count -1, stepDifference))).First();                 
            }            
               
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