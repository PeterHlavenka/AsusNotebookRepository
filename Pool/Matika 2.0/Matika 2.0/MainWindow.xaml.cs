using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Matika_2._0.Annotations;
using Mediaresearch.Framework.Gui;

namespace Matika_2._0
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Example m_example;
        private string m_userResult;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Example = new Example().Generate(10);

            GenerateCommand = new RelayCommand(DoGenerate);
            ResetCommand = new RelayCommand(DoReset);
        }

        public ICommand GenerateCommand { get; }
        public ICommand ResetCommand { get; }


        public Example Example
        {
            get => m_example;
            set
            {
                m_example = value;
                OnPropertyChanged(nameof(Example));
            }
        }

        public string UserResult
        {
            get => m_userResult;
            set
            {
                m_userResult = value;
                OnPropertyChanged(nameof(UserResult));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoReset()
        {
            UserResult = string.Empty;
        }

        private void DoGenerate(object obj)
        {
            var success = int.TryParse(obj.ToString(), out var number);
            if (success && number == Example.Result)
            {
                Example = Example.Generate(100);
                UserResult = string.Empty;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}