using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DoubleTextBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
