using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using Matika.Examples;
using Matika.Settings;
using Mediaresearch.Framework.Gui;

namespace Matika.Gui
{
    public class BigNumbersViewModel: MatikaViewModelBase
    {
        public BigNumbersViewModel(BigNumbersSettingsViewModel bigNumberSettingsViewModel)
        {
            Settings = bigNumberSettingsViewModel;

            Example = new BigNumbersAddition(Settings);
            
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
            
        }

        private BigNumbersSettingsViewModel Settings { get; set; }

        public override void SettingsButtonClicked()
        {
            var manager = new WindowManager();

            if (manager.ShowDialog(Settings) == false)
            {
                DoGenerate(Example.Result);
                SuccesCount--;
                ResultTextBox?.Focus();
            }
        }

        public override void DoGenerate(object obj)
        {
            var success = int.TryParse(obj.ToString(), out var number);
            if (success && number == Example.Result)
            {
                if (Repair == false)
                {
                    SuccesCount++;
                }

                Repair = false;

                Example = new BigNumbersAddition(Settings);

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
