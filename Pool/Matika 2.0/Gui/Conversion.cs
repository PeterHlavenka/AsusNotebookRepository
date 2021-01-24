using System;
using System.Collections.Generic;
using System.Linq;
using Matika.Gui;

namespace Matika
{
    public class Conversion
    {
        private Random rand = new Random();
        public double Result { get; private set; }
        public string TaskString { get; set; }
        protected static string EqualSign => " =  ";
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }
        public IConvertable SelectedConvertable { get; set; }

       

        public override bool Equals(object obj)
        {
            var conv = obj as Conversion;
            return conv.TaskString == TaskString;
        }


        public Conversion Generate(UnitConversionsSettingsViewModel settings)
        {
            var allowedConvertables = settings.Convertables.Where(d => d.IsEnabled).ToList();
            SelectedConvertable = allowedConvertables.ElementAt(rand.Next(0, allowedConvertables.Count));
            var dict = SelectedConvertable.UnitsDictionary;
            var step = SelectedConvertable.Step;
            var notNulls = dict.Where(d => d.Value != string.Empty).ToList();
            var stepDifference = settings.StepDifference;
            var from = notNulls.Skip(new Random().Next(notNulls.Count)).First();
            var highest = notNulls.Where(d => d.Key > from.Key).OrderBy(d => d.Key).ToList();
            var lowest = notNulls.Where(d => d.Key < from.Key).OrderByDescending(d => d.Key).ToList();
            var availables = highest.Take(stepDifference).ToList();
            availables.AddRange(lowest.Take(stepDifference));
            var to = availables.ElementAt(rand.Next(availables.Count));
            
            var stepsBetween = from.Key < to.Key ? to.Key - from.Key : from.Key - to.Key;
            var multiplier = 1;
            if (from.Key < to.Key && !settings.DecimalNumbers) multiplier = (int) Math.Pow(step, stepsBetween);


            var number = 0;
            number = rand.Next(1, 1 + SelectedConvertable.MaxDifficulty);
            number = number * multiplier;

            FromUnit = from.Value;
            ToUnit = to.Value;
            TaskString = string.Join(" ", number, FromUnit, EqualSign);
            Result = from.Key < to.Key ? number / Math.Pow(step, to.Key - @from.Key) : number * Math.Pow(step, @from.Key - to.Key);
         
            return this;
        }
    }
}