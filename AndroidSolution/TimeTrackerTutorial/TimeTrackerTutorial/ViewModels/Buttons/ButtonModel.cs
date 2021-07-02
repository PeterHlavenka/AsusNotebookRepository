using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTrackerTutorial.ViewModels.Buttons
{
    public class ButtonModel: ExtendedBindableObject
    {
        private string m_text;
        private bool m_isVisible;
        private ICommand m_command;
        private bool m_isEnabled;

        public string Text
        {
            get => m_text;
            set => SetProperty(ref m_text, value);
        }

        public bool IsVisible
        {
            get => m_isVisible;
            set => SetProperty(ref m_isVisible, value);
        }

        public bool IsEnabled
        {
            get => m_isEnabled;
            set => SetProperty(ref m_isEnabled, value);
        }

        public ICommand Command
        {
            get => m_command;
            set => SetProperty(ref m_command, value);
        }

        public ButtonModel(string text, ICommand command, bool isVisible = true, bool isEnabled = true)
        {
            m_text = text;
            m_command = command;
            m_isVisible = isVisible;
            m_isEnabled = isEnabled;
        }
        
        public ButtonModel(string text, Action action, bool isVisible = true, bool isEnabled = true)
        {
            m_text = text;
            m_command = new Command(action);
            m_isVisible = isVisible;
            m_isEnabled = isEnabled;
        }
    }
}