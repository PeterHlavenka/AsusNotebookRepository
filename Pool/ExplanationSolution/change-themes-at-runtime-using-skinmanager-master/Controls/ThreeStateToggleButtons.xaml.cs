using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataGrid_Themes.Controls
{
    public partial class ThreeStateToggleButtons : INotifyPropertyChanged
    {
        public ThreeStateToggleButtons()
        {
            InitializeComponent();
        }
        
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
            typeof(bool?), typeof(ThreeStateToggleButtons), new FrameworkPropertyMetadata(null, ValueChanged));
        
        public bool? Value
        {
            get => (bool?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        
        public bool IsFalseSelected => Value == false;
        public bool IsTrueSelected => Value == true;

        private static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ThreeStateToggleButtons button)
            {
                button.OnPropertyChanged(nameof(IsFalseSelected));
                button.OnPropertyChanged(nameof(IsTrueSelected));
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void FalseDown(object sender, MouseButtonEventArgs e)
        {
            if (Value == false)
            {
                Value = null;
                return;
            }

            Value = false;
        }

        private void TrueDown(object sender, MouseButtonEventArgs e)
        {
            if (Value == true)
            {
                Value = null;
                return;
            }

            Value = true;
        }
    }
}