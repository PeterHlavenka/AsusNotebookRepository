using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<PlotInfo> m_missingIntervals;
        private ObservableCollection<PlotInfo> m_availableInterval;

        public IntervalChartDatacontext()
        {
            AvailableInterval = new ObservableCollection<PlotInfo>
            {      
                //nepotrebuju vynechani
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 55, 30 ), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 58, 00 ), YVal = 7,}, 
                //null na vynechani staci jeden point
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = null,},               
                //dalsi interval
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 06, 00 ), YVal = 7,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 06, 00 ), YVal = null,},

                //custom = 8
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 50, 00 ), YVal = 8,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 06, 00 ), YVal = 8,},
            };

            MissingInterval = new ObservableCollection<PlotInfo>
            {
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 50, 00 ), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 55, 30 ), YVal = 6,},
                //null na vynechani
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 55, 30 ), YVal = null,},                
                //dalsi interval
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 58, 00 ), YVal = 6,},
                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = 6,},

                new PlotInfo {XDate = new DateTime(2013, 1, 26, 00, 03, 00 ), YVal = null,},
                //custom = 8
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 52, 00 ), YVal = 8,},
                new PlotInfo {XDate = new DateTime(2013, 1, 25, 23, 53, 00 ), YVal = 8,},
            };


            //LineSeries line = new LineSeries
            //{
            //    Stroke = new SolidColorBrush {Color = Colors.GreenYellow},               
            //    CategoryBinding = new PropertyNameDataPointBinding() {PropertyName = "XDate" },
            //    ValueBinding = new PropertyNameDataPointBinding() {PropertyName = "YVal" },
            //    ItemsSource = AvailableInterval,
                
            //};

            //LineSeries missing = new LineSeries
            //{
            //    Stroke = new SolidColorBrush { Color = Colors.Red },               
            //    CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "XDate" },
            //    ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YVal" },
            //    ItemsSource = MissingInterval
            //};


            //intervalChart.Series.Add(line);
            //intervalChart.Series.Add(missing);
        }


        public ObservableCollection<PlotInfo> MissingInterval
        {
            get { return m_missingIntervals; }
            set { m_missingIntervals = value; NotifyOfPropertyChange();}
        }

        public ObservableCollection<PlotInfo> AvailableInterval
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
