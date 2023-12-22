using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Syncfusion.UI.Xaml.Charts;

namespace SfChartFontInReview;

public partial class LineChart : UserControl
{
    private const Orientation LegendOrientation = Orientation.Horizontal;
    private const double MaxLegendHeight = 200d;
    private const double MaxLegendWidth = 150d;

    public LineChart()
    {
        InitializeComponent();

        // Create a new chart control
        var chart = new SfChart();
        chart.Margin = new Thickness(20);
        chart.Width = 800;
        chart.Height = 500;

        // Create a primary axis (X-axis)
        var primaryAxis = new CategoryAxis();
        chart.PrimaryAxis = primaryAxis;

        // Create a secondary axis (Y-axis)
        var secondaryAxis = new NumericalAxis();
        chart.SecondaryAxis = secondaryAxis;

        // Create a series (LineSeries in this example)
        var series = new LineSeries();
        series.ItemsSource = GetChartData(); // Provide your data source here
        series.XBindingPath = "XValue"; // Property for X-values
        series.YBindingPath = "YValue"; // Property for Y-values
        chart.Series.Add(series);
        chart.Legend = CreateLegend();

        // Add the chart to the Grid
        Grid.Children.Add(chart);
    }

    private ChartLegend CreateLegend()
    {
        var legend = new ChartLegend();
        legend.DataContext = this;
        // legend.DockPosition = m_legendPosition;

        var wrapPanelFactory = new FrameworkElementFactory(typeof(WrapPanel));
        wrapPanelFactory.SetBinding(MaxWidthProperty, new Binding(nameof(MaxLegendWidth)));
        wrapPanelFactory.SetBinding(MaxHeightProperty, new Binding(nameof(MaxLegendHeight)));
        wrapPanelFactory.SetBinding(WrapPanel.OrientationProperty, new Binding(nameof(LegendOrientation)));

        legend.ItemsPanel = new ItemsPanelTemplate { VisualTree = wrapPanelFactory };
        return legend;
    }

    private ChartData[] GetChartData()
    {
        return new ChartData[]
        {
            new("Jan", 30),
            new("Feb", 45),
            new("Mar", 25),
            new("Apr", 60),
            new("May", 50),
            new("Jun", 75)
        };
    }
}

public class ChartData
{
    public ChartData(string xValue, double yValue)
    {
        XValue = xValue;
        YValue = yValue;
    }

    public string XValue { get; set; }
    public double YValue { get; set; }
}