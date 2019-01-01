using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Mediaresearch.Framework.Gui;

namespace Matika_2._0
{
    public class MainViewModel : Screen
    {
        private int m_counter;
        private Example m_example;
        private SettingsDialogViewModel m_settings;

        private int m_succesCount;
        private string m_userResult;

        private int m_wrongCount;


        public MainViewModel()
        {
            Counter = 0;
            SuccesCount = 0;
            WrongCount = 0;
            Settings = new SettingsDialogViewModel {Difficulty = 10};
            Example = new Example().Generate(Settings);
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        public BitmapImage Img => new BitmapImage(new Uri(@"pack://application:,,,/Matika 2.0;component/Resources/Settings.ico"));

        public SettingsDialogViewModel Settings
        {
            get => m_settings;
            set
            {
                m_settings = value;
                NotifyOfPropertyChange();
            }
        }


        public ICommand GenerateCommand { get; }
        public ICommand ResetCommand { get; }


        public Example Example
        {
            get => m_example;
            set
            {
                m_example = value;
                Counter++;
                NotifyOfPropertyChange();
            }
        }

        public string UserResult
        {
            get => m_userResult;
            set
            {
                m_userResult = value;
                NotifyOfPropertyChange();
            }
        }

        public int Counter
        {
            get => m_counter;
            set
            {
                m_counter = value;
                NotifyOfPropertyChange();
            }
        }

        public int SuccesCount
        {
            get => m_succesCount;
            set
            {
                m_succesCount = value;
                NotifyOfPropertyChange();
            }
        }

        public int WrongCount
        {
            get => m_wrongCount;
            set
            {
                m_wrongCount = value;
                NotifyOfPropertyChange();
            }
        }

        private void DoReset()
        {
            UserResult = string.Empty;
            WrongCount++;
        }


        private void DoGenerate(object obj)
        {                  
            var success = int.TryParse(obj.ToString(), out var number);
            if (success && number == Example.Result)
            {                
                SuccesCount++;

                Example temp;
                do
                {
                    temp = Example.Generate(Settings);
                } while (Example.Task == temp.Task);

                Example = temp;

                UserResult = string.Empty;
            }
        }

        public void SettingsButtonClicked()
        {
            var manager = new WindowManager();

            if (manager.ShowDialog(Settings) == false)
            {
                DoGenerate(Example.Result);
                Counter--;
            }
        }
    }
}