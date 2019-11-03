using System.Windows.Media;
using Caliburn.Micro;
using Mediaresearch.Framework.Gui;

namespace Matika.Gui
{
    public class MatikaViewModel : ViewModelBase
    {
        private Example m_example;
        private MatikaSettingsViewModel m_settings;

        public MatikaViewModel(int difficulty)
        {
            Counter = 0;
            SuccesCount = 0;
            WrongCount = 0;

            Settings = new MatikaSettingsViewModel {Difficulty = difficulty};
            Example = new Example().Generate(Settings);
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        public Example Example
        {
            get => m_example;
            set
            {
                m_example = value;
                Counter++;
                ResultBrush = Brushes.Black;
                NotifyOfPropertyChange();
            }
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
            var success = int.TryParse(obj.ToString(), out var number);
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


        public void SettingsButtonClicked()
        {
            var manager = new WindowManager();

            if (manager.ShowDialog(Settings) == false)
            {
                DoGenerate(Example.Result);
                Counter--;
                SuccesCount--;
                ResultTextBox.Focus();
            }
        }
    }
}