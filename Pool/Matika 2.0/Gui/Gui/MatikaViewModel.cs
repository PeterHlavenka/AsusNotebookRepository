using System.Windows.Media;
using Caliburn.Micro;
using Matika.Examples;
using Matika.Settings;
using Mediaresearch.Framework.Gui;

namespace Matika.Gui
{
    public class MatikaViewModel : MatikaViewModelBase
    {
        private MatikaSettingsViewModel m_settings;

        public MatikaViewModel(int difficulty, int addCount, int differenceCount, int productCount, int divideCount)
        {
            SuccesCount = 0;
            WrongCount = 0;

            Settings = new MatikaSettingsViewModel {Difficulty = difficulty, AddCount = addCount, DifferenceCount = differenceCount, ProductCount = productCount, DivideCount = divideCount};
            Example = new Example().Generate(Settings);
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        public MatikaSettingsViewModel Settings
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
            bool success = int.TryParse(obj.ToString(), out int number);
            if (success && number == Example.Result)
            {
                if (Repair == false)
                {
                    SuccesCount++;
                }

                Repair = false;

                Example temp;
                do
                {
                    temp = Example.Generate(Settings);
                } while (Example.TaskString == temp.TaskString);

                Example = temp;

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


        public override void SettingsButtonClicked()
        {
            var manager = new WindowManager();

            if (manager.ShowDialog(Settings) == false)
            {
                DoGenerate(Example.Result);
                SuccesCount--;
                ResultTextBox.Focus();
            }
        }
    }
}