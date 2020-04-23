using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace RadCartesianChartTest
{
    public class IntervalChartDatacontext : Screen
    {
        private List<PlotInfo> m_missingIntervals;
        private List<PlotInfo> m_availableInterval;

        public IntervalChartDatacontext(RadCartesianChart intervalChart)
        {
            AvailableInterval = new List<PlotInfo>
            {
                new PlotInfo {XDate = new DateTime(2013, 1, 22), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 23), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 24), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 27), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 28), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 29), YVal = null,},
            };

            MissingInterval = new List<PlotInfo>
            {
                new PlotInfo {XDate = new DateTime(2013, 1, 22), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 23), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 24), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 27), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 28), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 29), YVal = 7,},
            };

            NextChannel = new List<PlotInfo>
            {
                new PlotInfo {XDate = new DateTime(2013, 1, 22), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 23), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 24), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 27), YVal = null,},
                new PlotInfo {XDate = new DateTime(2013, 1, 28), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 29), YVal = 6,},
            };

            LineSeries line = new LineSeries
            {
                Stroke = new SolidColorBrush {Color = Colors.GreenYellow},               
                CategoryBinding = new PropertyNameDataPointBinding() {PropertyName = "XDate" },
                ValueBinding = new PropertyNameDataPointBinding() {PropertyName = "YVal" },
                ItemsSource = AvailableInterval
            };

            LineSeries missing = new LineSeries
            {
                Stroke = new SolidColorBrush { Color = Colors.Red },               
                CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "XDate" },
                ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YVal" },
                ItemsSource = NextChannel
            };

            intervalChart.Series.Add(line);
            intervalChart.Series.Add(missing);
        }

        public List<PlotInfo> NextChannel { get; set; }

        public List<PlotInfo> MissingInterval
        {
            get { return m_missingIntervals; }
            set { m_missingIntervals = value; NotifyOfPropertyChange();}
        }

        public List<PlotInfo> AvailableInterval
        {
            get { return m_availableInterval; }
            set { m_availableInterval = value; NotifyOfPropertyChange();}
        }
    }

    public class PlotInfo
    {
        public DateTime XDate { get; set; }
        public double? YVal { get; set; }
    }
}
