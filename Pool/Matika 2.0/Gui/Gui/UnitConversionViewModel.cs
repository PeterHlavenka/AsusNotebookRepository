using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Mediaresearch.Framework.Gui;

namespace Matika.Gui
{
    public class UnitConversionViewModel : ViewModelBase
    {
        private Conversion m_conversion;
        private UnitConversionsSettingsViewModel m_settings;
        private Visibility m_helpImageVisibility = Visibility.Hidden ;
        private Visibility m_helpButtonVisibility = Visibility.Visible;
        private BitmapImage m_helpImage;
        public new TextBox ResultTextBox { get; set; }

        public UnitConversionViewModel(int difficulty, Conversion conversion, IEnumerable<IConvertable> convertables)
        {
            Counter = 0;
            SuccesCount = 0;
            WrongCount = 0;

            DisplayName = "Nastavení";
            Settings = new UnitConversionsSettingsViewModel (convertables) { Difficulty = difficulty};
            Conversion = conversion.Generate(Settings);
            Settings.Difficulty = Math.Min(Settings.Difficulty,  Conversion.SelectedConvertable.MaxDifficulty);
            HelpImage = Conversion.SelectedConvertable.HelpImage;
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        private void SettingsSelectionChanged(object sender, EventArgs e)
        {
            
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
            var success = decimal.TryParse(obj.ToString().Replace(',', '.'), out var number);
            var test = obj.ToString().Split(new[] {','});

            var result = (decimal)Conversion.Result;
            if (test.Length > 1)
            {
                var len =  test[1].Length;
                result = Math.Round(result, len); //.ToString().Replace('.', ',');
               
            }
            
            if (!success || number < result || number > result)  
            {
                if (!string.IsNullOrEmpty(obj.ToString()))
                {
                    ResultBrush = Brushes.Red;
                    Repair = true;
                }
            }
            else
            {
                if (Repair == false)
                {
                    SuccesCount++;
                }

                Repair = false;

                Conversion = Conversion.Generate(Settings);
                HelpImage = Conversion.SelectedConvertable.HelpImage;
                HelpImageVisibility = Visibility.Hidden;
                Settings.Difficulty = Math.Min(Settings.Difficulty,  Conversion.SelectedConvertable.MaxDifficulty);

                UserResult = string.Empty;
            }

            
        }

        public void SettingsButtonClicked()
        {
            var manager = new WindowManager();

            if (manager.ShowDialog(Settings) == false)
            {
                if (!Settings.Convertables.Any(d => d.IsEnabled))
                {
                    Settings.Convertables.First().IsEnabled = true;
                }
                Conversion = Conversion.Generate(Settings);
                HelpImage = Conversion.SelectedConvertable.HelpImage;
                HelpImageVisibility = Visibility.Hidden;
                if (Counter > 0)
                {
                    Counter--;
                    SuccesCount--;
                }

                ResultTextBox.Focus();
            }
        }

        public void TestButtonClicked()
        {
             var result = (decimal)Conversion.Result;
             DoGenerate(result);
        }
        
        public void HelpButtonClicked()
        {
            
            HelpImageVisibility = HelpImageVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        public Visibility HelpImageVisibility
        {
            get => m_helpImageVisibility;
            set
            {
                m_helpImageVisibility = value;
                HelpButtonVisibility = m_helpImageVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                NotifyOfPropertyChange();
            }
        }
        
        public Visibility HelpButtonVisibility
        {
            get => m_helpButtonVisibility;
            set
            {
                m_helpButtonVisibility = value; 
                NotifyOfPropertyChange();
            }
        }

        public BitmapImage HelpImage
        {
            get => m_helpImage;
            set
            {
                m_helpImage = value;
                NotifyOfPropertyChange();
            }
        }
    }
}