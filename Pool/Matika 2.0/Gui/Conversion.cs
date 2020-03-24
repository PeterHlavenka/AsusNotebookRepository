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
            var allowedConvertables = Convertables.First(); // delka zatim
            Dictionary<int, string> dict = allowedConvertables.UnitsDictionary;
            int step = allowedConvertables.Step;
            List<KeyValuePair<int, string>> notNulls = dict.Where(d => d.Value != string.Empty).ToList();

            int number = new Random().Next(1, 1 + settings.Difficulty * settings.Difficulty * settings.Difficulty);
            int stepDifference = settings.StepDifference;

            KeyValuePair<int, string> from = notNulls.Skip(new Random().Next(notNulls.Count)).First();

            List<KeyValuePair<int, string>> highest = notNulls.Where(d => d.Key > from.Key).OrderBy(d => d.Key).ToList();
            List<KeyValuePair<int, string>> lowest = notNulls.Where(d => d.Key < from.Key).OrderByDescending(d => d.Key).ToList();

            List<KeyValuePair<int, string>> availables = highest.Take(stepDifference).ToList();
            availables.AddRange(lowest.Take(stepDifference));

            KeyValuePair<int, string> to = availables.ElementAt(rand.Next(availables.Count));

            Result = from.Key < to.Key ? number / Math.Pow(step, to.Key - @from.Key) : number * Math.Pow(step, @from.Key - to.Key);

            while (!settings.DecimalNumbers && Math.Abs(Result - (int) Result) > double.Epsilon)
            {
                number = number * 10;
                Result = from.Key < to.Key ? number / Math.Pow(step, to.Key - @from.Key) : number * Math.Pow(step, @from.Key - to.Key);
            }

            FromUnit = from.Value;
            ToUnit = to.Value;
            TaskString = string.Join(" ", number, FromUnit, EqualSign);

            return this;
        }
    }
}