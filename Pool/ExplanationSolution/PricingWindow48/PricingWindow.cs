using System;
using System.Threading;
using System.Windows;

namespace PricingWindow48
{
    /// <summary>
    /// Interaction logic for PricingWindow.xaml
    /// </summary>
    public partial class PricingWindow
    {
        public PricingWindow()
        {
            InitializeComponent();
            DataContext = new PricingWindowViewModel();
        }

        private void PricingWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is PricingWindowViewModel model)
            {
                Console.WriteLine();
            }

            TextBlock.Text = Thread.CurrentThread.ManagedThreadId.ToString();

        }
    }
}