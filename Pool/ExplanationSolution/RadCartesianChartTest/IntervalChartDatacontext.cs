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
                //nepotrebuju vynechani
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 55, 30 ), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 58, 00 ), YVal = 7,}, 
                //null na vynechani staci jeden point
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = null,},               
                //dalsi interval
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 06, 00 ), YVal = 7,},
            };

            MissingInterval = new List<PlotInfo>
            {
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 50, 00 ), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 55, 30 ), YVal = 6,},
                //null na vynechani
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 55, 30 ), YVal = null,},                
                //dalsi interval
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 58, 00 ), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = 6,},
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
                ItemsSource = MissingInterval
            };


            intervalChart.Series.Add(line);
            intervalChart.Series.Add(missing);         
        }


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
