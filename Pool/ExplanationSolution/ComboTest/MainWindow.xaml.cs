using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ComboTest.Annotations;

namespace ComboTest
{
    /// <summary>
    ///     Pridava do ComboItems stringy zadane uzivatelem z UI na trigger LostFocus
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<string> m_items;
        private string m_text;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Items
        {
            get => m_items;
            set
            {
                m_items = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => m_text;
            set
            {
                m_text = value;
                OnPropertyChanged();

                Items.Add(m_text);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}