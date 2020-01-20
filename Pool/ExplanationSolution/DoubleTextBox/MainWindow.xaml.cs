using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace DoubleTextBox
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double m_value = 0.5d;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("cs-CZ");
        }

        public double Value
        {
            get => m_value;
            set
            {
                m_value = value;
                Console.WriteLine();
            }
        }

        public CultureInfo Info => CultureInfo.CreateSpecificCulture("cs-CZ");
    }
}