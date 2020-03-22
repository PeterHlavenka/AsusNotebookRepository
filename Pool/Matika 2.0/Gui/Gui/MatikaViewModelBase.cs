﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using Matika.Examples;
using WpfAnimatedGif;

namespace Matika.Gui
{
    public abstract class MatikaViewModelBase : Screen
    {
        private Visibility m_monkeyVisibility = Visibility.Collapsed;
        private Brush m_resultBrush = Brushes.Black;
        private int m_succesCount;
        private string m_userResult;
        private int m_wrongCount;
        public TextBox ResultTextBox { get; set; }
        public ICommand GenerateCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        private Example m_example;

        public abstract void SettingsButtonClicked();

        public Example Example
        {
            get => m_example;
            set
            {
                m_example = value;
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


        protected bool Repair { get; set; }

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

        public abstract void DoGenerate(object obj);

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

        protected void DoReset()
        {
            if (string.IsNullOrEmpty(UserResult))
            {
                return;
            }

            UserResult = UserResult.Substring(0, UserResult.Length - 1);

            if (Equals(ResultBrush, Brushes.Red))
            {
                WrongCount++;
                ResultBrush = Brushes.Black;

                if (!IsMonkeyRunning)
                {
                    MoveMonkey();
                }

                UserResult = string.Empty;
            }
        }
    }
}