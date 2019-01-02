using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using Mediaresearch.Framework.Gui;
using WpfAnimatedGif;

namespace Matika
{
    public class MainViewModel : Screen
    {
        private int m_counter;
        private Example m_example;

        private Visibility m_monkeyVisibility = Visibility.Collapsed;

        private Brush m_resultBrush;
        private SettingsDialogViewModel m_settings;

        private int m_succesCount;
        private string m_userResult;

        private int m_wrongCount;


        public MainViewModel(int difficulty)
        {
            Counter = 0;
            SuccesCount = 0;
            WrongCount = 0;

            Settings = new SettingsDialogViewModel {Difficulty = difficulty};
            Example = new Example().Generate(Settings);
            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }


        public TextBox ResultTextBox { get; set; }

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
                ResultBrush = Brushes.Black;
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

        public Brush ResultBrush
        {
            get => m_resultBrush;
            set
            {
                m_resultBrush = value;
                NotifyOfPropertyChange();
            }
        }

        private DispatcherTimer Timer { get; set; }


        private bool Repair { get; set; }

        public ImageAnimationController MonkeyController { get; set; }
        private bool IsMonkeyRunning { get; set; }

        public Visibility MonkeyVisibility
        {
            get => m_monkeyVisibility;
            set
            {
                m_monkeyVisibility = value;
                NotifyOfPropertyChange();
            }
        }

        private void MoveMonkey()
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(6),
                IsEnabled = true
            };
            Timer.Tick += PauseMonkey;

            Timer.Start();
            IsMonkeyRunning = true;
            MonkeyVisibility = Visibility.Visible;
            SetMonkeyStartPosition();
            MonkeyController.Play();
        }


        private void SetMonkeyStartPosition()
        {
            MonkeyController.GotoFrame(29);
        }

        private void PauseMonkey(object sender, EventArgs e)
        {
            IsMonkeyRunning = false;
            MonkeyController.Pause();
            Timer.Stop();
        }

        private void DoReset()
        {
            if (string.IsNullOrEmpty(UserResult))
            {
                return;
            }

            UserResult = string.Empty;
           
            if (Equals(ResultBrush, Brushes.Red))
            {
                WrongCount++;
                ResultBrush = Brushes.Black;

                if (!IsMonkeyRunning)
                {
                    MoveMonkey();
                }
            }          
        }

        private void DoGenerate(object obj)
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
                } while (Example.Task == temp.Task);

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