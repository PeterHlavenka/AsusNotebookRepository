using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Caliburn.Micro;
using Mediaresearch.Framework.Gui;

namespace Matika.Gui
{
    public class UnitConversionViewModel : ViewModelBase
    {
        private Conversion m_conversion;
        private UnitConversionsSettingsViewModel m_settings;
        public new TextBox ResultTextBox { get; set; }

        public UnitConversionViewModel(int difficulty, Conversion conversion, IEnumerable<IConvertable> convertables)
        {
            Counter = 0;
            SuccesCount = 0;
            WrongCount = 0;

            DisplayName = "Nastavení";
            Settings = new UnitConversionsSettingsViewModel (convertables) { Difficulty = difficulty};
            Conversion = conversion.Generate(Settings); 
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        public Conversion Conversion
        {
            get => m_conversion;
            set
            {
                m_conversion = value;
                NotifyOfPropertyChange();
            } 
        }

        public UnitConversionsSettingsViewModel Settings
        {
            get => m_settings;
            set
            {
                m_settings = value;
                NotifyOfPropertyChange();
            }
        }

        public override void DoGenerate(object obj)
        {
            var success = double.TryParse(obj.ToString(), out var number);

            if (success && Math.Abs(number - Conversion.Result) < double.Epsilon)
            {
                if (Repair == false)
                {
                    SuccesCount++;
                }

                Repair = false;

                Conversion = Conversion.Generate(Settings); 

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

        public void SettingsButtonClicked()
        {
            var manager = new WindowManager();

            if (manager.ShowDialog(Settings) == false)
            {
                Conversion = Conversion.Generate(Settings);
                Counter--;
                SuccesCount--;
                ResultTextBox.Focus();
            }
        }
    }
}