using System;
using System.Collections.Generic;
using System.Linq;

namespace Matika.Gui
{
   public class UnitConversionsSettingsViewModel : SettingsBase
    {
        private bool m_decimalNumbers;
        private int m_stepDifference = 2;


        public UnitConversionsSettingsViewModel(IEnumerable<IConvertable> convertables)
        {
            Convertables = convertables;
            Convertables.First().IsEnabled = true;
            DisplayName = "Nastavení";
        }

        public override int Difficulty
        {
            get => m_difficulty;
            set
            {
                var selectedConvertables = Convertables.Where(d => d.IsEnabled).ToList();
                var minimum = selectedConvertables.Min(d => d.MaxDifficulty);
                m_difficulty = value > minimum ? minimum : value;
                NotifyOfPropertyChange();
            }
        }

        public bool DecimalNumbers
        {
            get => m_decimalNumbers;
            set
            {
                m_decimalNumbers = value;
                NotifyOfPropertyChange();
            }
        }

        public int StepDifference
        {
            get => m_stepDifference;
            set
            {
                m_stepDifference = value;
                NotifyOfPropertyChange();
            }
        }

        public void Checked()
        {
            var selectedConvertables = Convertables.Where(d => d.IsEnabled).ToList();
            var minimum = selectedConvertables.Min(d => d.MaxDifficulty);
            Difficulty = Difficulty > minimum ? minimum : Difficulty;
        }
        
        public IEnumerable<IConvertable> Convertables { get; set; }
    }
}
