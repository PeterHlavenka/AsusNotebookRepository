using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace SfChartFontInReview;

public partial class ColumnChart : UserControl
{
    public ColumnChart()
    {
       
        Data = new List<Person>
        {
            new() { Name = "David", Height = 180 },
            new() { Name = "Michael", Height = 170 },
            new() { Name = "Steve", Height = 160 },
            new() { Name = "Joel", Height = 182 }
        };
        InitializeComponent();
        ColumnSeries.ItemsSource = Data;
       
    }

    public List<Person> Data { get; set; }
}

public class Person
{
    public string Name { get; set; }

    public double Height { get; set; }
}