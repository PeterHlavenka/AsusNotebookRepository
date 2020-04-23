using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Caliburn.Micro;

namespace RadCartesianChartTest
{
    public class OlympicMedalStatisticsViewModel : Screen
    {
        private IEnumerable<OlympicStatisticsReport> m_allData;

     

        public OlympicMedalStatisticsViewModel()
        {            
            AllData = new List<OlympicStatisticsReport>
           {
               new OlympicStatisticsReport
               {
                   CountryName = "Krumlov", 
                   NOC = "neco",
                   Gold = 3,
                   Bronze = 2,
                   Silver = 1
               },
               new OlympicStatisticsReport
               {
                   CountryName = "Brno",
                   NOC = "neco jin",
                   Gold = 4,
                   Bronze = 5,
                   Silver = 3
               },
           };   
        }

        public IEnumerable<OlympicStatisticsReport> AllData
        {
            get { return m_allData; }
            set
            {
                if (m_allData == value)
                {
                    return;
                }

                m_allData = value;
                NotifyOfPropertyChange();
            }
        }
    }
}