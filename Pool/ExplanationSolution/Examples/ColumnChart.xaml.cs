using System.Collections.Generic;
using System.Linq;
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
        //CalculateMargin();
       
    }

    public List<Person> Data { get; set; }

    private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var list = Data.ToList();
        list.Add(new Person { Name = "Added", Height = 150 });

        Data = list;
        ColumnSeries.ItemsSource = Data;
        //CalculateMargin();
    }

    private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var list = Data.ToList();
        list.RemoveAt(Data.Count - 1);

        Data = list;
        ColumnSeries.ItemsSource = Data;
        //CalculateMargin();
    }

    private void CalculateMargin()
    {
        var left = 0d;
        var chartWidth = 1000;
        var columnCount = Data.Count;

        if(columnCount > 0)
        {
            left = (chartWidth / columnCount) / 2;
            // remove 14% of left value
            left = -(left - (left * 0.14));
        }



        ColumnSeries.Margin = new System.Windows.Thickness(left, 0, 0, 0);
    }
}

public class Person
{
    public string Name { get; set; }

    public double Height { get; set; }
}