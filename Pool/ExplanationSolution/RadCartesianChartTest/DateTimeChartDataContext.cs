using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace RadCartesianChartTest
{
    public class DateTimeChartDataContext :  Screen
    {
        private List<ReferenceStreamData> m_allData;

        public DateTimeChartDataContext(RadCartesianChart chart1)
        {
            DateTime lastDate = DateTime.Now;
            double[] lastVal = new double[] { 20, 20, 20, 20, 20 };

            AllData = new List<ReferenceStreamData>();

            
            var first = new ReferenceStreamData{Date = DateTime.Now, Value = 15};
            var second = new ReferenceStreamData { Date = DateTime.Now.AddMinutes(10), Value = 13 };

            AllData.Add(first);
            AllData.Add(second);

            //for (int i = 0; i < 5; ++i)
            //{
            //    ReferenceStreamData obj = new ReferenceStreamData { Date = lastDate.AddMinutes(1), Value = lastVal[i], ChannelName = "Ahoj"};
            //    AllData.Add(obj);
            //    lastDate = obj.Date;
            //}
            LineSeries validIntervals = (LineSeries)chart1.Series[0];
            validIntervals.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            validIntervals.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };

            LineSeries missingIntervals = (LineSeries)chart1.Series[1];
            missingIntervals.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            missingIntervals.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
            //series.ItemsSource = AllData;
        }

        public List<ReferenceStreamData> AllData
        {
            get { return m_allData; }
            set
            {
                m_allData = value; 
                NotifyOfPropertyChange();
            }
        }
    }

    public class ReferenceStreamData
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string ChannelName { get; set; }
    }
}
