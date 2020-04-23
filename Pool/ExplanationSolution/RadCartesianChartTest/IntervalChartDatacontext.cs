using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace RadCartesianChartTest
{
    public class IntervalChartDatacontext
    {
        public IntervalChartDatacontext(RadCartesianChart intervalChart)
        {
            intervalChart.DataContext = new List<PlotInfo>
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
        }
    }

    public class PlotInfo
    {
        public DateTime XDate { get; set; }
        public double? YVal { get; set; }
    }
}
