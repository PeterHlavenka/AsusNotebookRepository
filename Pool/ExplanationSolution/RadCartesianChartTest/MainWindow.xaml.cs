using System;
using System.Collections.Generic;

namespace RadCartesianChartTest
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
       

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new OlympicMedalStatisticsViewModel();
            DateTimeChart.DataContext = new DateTimeChartDataContext(DateTimeChart);
            IntervalChart.DataContext = new IntervalChartDatacontext();
            BindedSeriesChart.DataContext = new BindedSeriesChartDatacontext();
        }
    }
}