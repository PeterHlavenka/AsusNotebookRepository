using System;
using System.Collections.Generic;

namespace RadCartesianChartTest
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int m_pocetDat;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            DataContext = new OlympicMedalStatisticsViewModel();
            DateTimeChart.DataContext = new DateTimeChartDataContext(DateTimeChart);
            IntervalChart.DataContext = new IntervalChartDatacontext();
            BindedSeriesChart.DataContext = new BindedSeriesChartDatacontext(BindedSeriesChart);
        }

        public int PocetDat
        {
            get => m_pocetDat;
            set
            {
                m_pocetDat = value;

                if (BindedSeriesChart.DataContext is BindedSeriesChartDatacontext context)
                {
                    context.PocetNacitanychDatDoGrafu = m_pocetDat;
                    context.Reload();
                }
            }
        }
    }
}