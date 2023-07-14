using System.Windows;
using Syncfusion.UI.Xaml.Charts;

namespace SfChartFontInReview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var chart = new SfChart();
            chart.PrimaryAxis = new DateTimeAxis();
            chart.SecondaryAxis = new NumericalAxis();
            chart.SecondaryAxis.FontSize = 30;
            Grid.Children.Add(chart);
        }
    }
}