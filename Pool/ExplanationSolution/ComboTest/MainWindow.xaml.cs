using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComboTest.Annotations;

namespace ComboTest
{
    /// <summary>
    /// Pridava do ComboItems stringy zadane uzivatelem z UI na trigger LostFocus 
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string m_text;
        private ObservableCollection<string> m_items;

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
