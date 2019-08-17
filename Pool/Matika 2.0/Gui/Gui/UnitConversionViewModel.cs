using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Mediaresearch.Framework.Gui;

namespace Matika.Gui
{
    public class UnitConversionViewModel : ViewModelBase
    {
        public TextBox ResultTextBox { get; set; }

        public UnitConversionViewModel(int difficulty, Conversion conversion)
        {
            Counter = 0;
            SuccesCount = 0;
            WrongCount = 0;

            Settings = new SettingsDialogViewModel { Difficulty = difficulty };
            Conversion = conversion.Generate(Settings); //new Conversion(new List<IConvertable>()).Generate(Settings);
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        public Conversion Conversion { get; set; }

        public override void DoGenerate(object obj)
        {



            var success = int.TryParse(obj.ToString(), out var number);
            if (success && number == Conversion.Result)
            {
                if (Repair == false)
                {
                    SuccesCount++;
                }

                Repair = false;

                Conversion temp;
                do
                {
                    temp = Conversion.Generate(Settings);
                } while (Conversion.TaskString == temp.TaskString);

                Conversion = temp;

                UserResult = string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(obj.ToString()))
                {
                    ResultBrush = Brushes.Red;
                    Repair = true;
                }
            }
        }
    }
}