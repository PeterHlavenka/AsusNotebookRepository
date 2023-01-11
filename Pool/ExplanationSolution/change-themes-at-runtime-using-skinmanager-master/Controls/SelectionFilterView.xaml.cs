using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace DataGrid_Themes.Controls
{
    public partial class SelectionFilterView : INotifyPropertyChanged
    {
        public const string False = "F";
        public const string True = "T";
        public const string Null = "N";


        public SelectionFilterView()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty TextFilterProperty = DependencyProperty.Register(nameof(TextFilter),
            typeof(string), typeof(SelectionFilterView), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SelectionFilterView) d).OnPropertyChanged(nameof(TextFilter));
        }

        public string TextFilter
        {
            get => (string) GetValue(TextFilterProperty);
            set
            {
                SetValue(TextFilterProperty, value?? string.Empty);
                OnPropertyChanged(nameof(IsFalseSelected));
                OnPropertyChanged(nameof(IsTrueSelected));
                OnPropertyChanged(nameof(IsOtherSelected));
            }
        }


        public bool IsFalseSelected
        {
            get => TextFilter?.Contains(False) ?? false;
            set
            {
                switch (value)
                {
                    case true:
                        TextFilter += False;
                        break;
                    case false:
                        TextFilter = TextFilter.Replace(False, string.Empty);
                        break;
                }
                RaiseTextFilterChanged();
            }
        }

        public bool IsTrueSelected
        {
            get => TextFilter?.Contains(True) ?? false;
            set
            {
                switch (value)
                {
                    case true:
                        TextFilter += True;
                        break;
                    case false:
                        TextFilter = TextFilter.Replace(True, string.Empty);
                        break;
                }
                RaiseTextFilterChanged();
            }
        }

        public bool IsOtherSelected
        {
            get => TextFilter?.Contains(Null) ?? false;
            set
            {
                switch (value)
                {
                    case true:
                        TextFilter += Null;
                        break;
                    case false:
                        TextFilter = TextFilter.Replace(Null, string.Empty);
                        break;
                }
                RaiseTextFilterChanged();
            }
        }


        public event EventHandler TextFilterChanged;


        private void RaiseTextFilterChanged()
        {
            TextFilterChanged?.Invoke(this, EventArgs.Empty);
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}