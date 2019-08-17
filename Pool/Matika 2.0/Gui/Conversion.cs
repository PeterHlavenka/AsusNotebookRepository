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

        public int Result { get; protected set; }
        public string TaskString { get; set; }
        protected static string EqualSign => " =  ";

        public IEnumerable<IConvertable> Convertables { get; set; }

        public Conversion Generate(SettingsDialogViewModel settings)
        {
            // allowed conversions.random
            var allowedConvertables = Convertables.First();

            var units = allowedConvertables.Units;

            var number = 3;
            var from = 0;
            var to = 0;
            var fromUnit = string.Empty;
            var toUnit = string.Empty;

            while (fromUnit == "null" || toUnit == "null" || from == to)
            {
                from = new Random().Next(units.Count);
                to = new Random().Next(units.Count);
                fromUnit = allowedConvertables.Units.ElementAt(from);
                toUnit = allowedConvertables.Units.ElementAt(to);

                if(from != to)
                Result = (int) (from < to ? number * Math.Pow(allowedConvertables.Step, (to - from)) : number / Math.Pow(allowedConvertables.Step, (from - to) ))  ;
                TaskString = string.Join(" ", number, fromUnit, EqualSign, toUnit);
            }

            return this;
        }
    }
}