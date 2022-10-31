using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimeTrackerTutorial.Models;
using TimeTrackerTutorial.ViewModels.Buttons;

namespace TimeTrackerTutorial.PageModels
{
    public class TimeClockPageModel  : PageModelBase
    {
        private TimeSpan m_runningTotal;
        private DateTime m_currentStartTime;
        private ObservableCollection<WorkItem> m_workItems;
        private double m_todayEarnings;
        private ButtonModel m_clockInOutButtonModel;

        public TimeSpan RunningTotal
        {
            get => m_runningTotal;
            set => SetProperty(ref m_runningTotal, value);
        }

        public DateTime CurrentStartTime
        {
            get => m_currentStartTime;
            set => SetProperty(ref m_currentStartTime, value);
        }

        public ObservableCollection<WorkItem> WorkItems
        {
            get => m_workItems;
            set => SetProperty(ref m_workItems, value);
        }

        public double TodayEarnings
        {
            get => m_todayEarnings;
            set => SetProperty(ref m_todayEarnings, value);
        }

        public ButtonModel ClockInOutButtonModel
        {
            get => m_clockInOutButtonModel;
            set => SetProperty(ref m_clockInOutButtonModel, value);
        }

        public TimeClockPageModel()
        {
            WorkItems = new ObservableCollection<WorkItem>();
            ClockInOutButtonModel = new ButtonModel("Clock In", OnClockInOutAction);
        }

        public override Task InitializeAsync(object navigationDate = null)
        {
            RunningTotal = new TimeSpan(0,0,0);
            TodayEarnings = default;
            return base.InitializeAsync(navigationDate);
        }

        private void OnClockInOutAction()
        {
            throw new NotImplementedException();
        }
    }
}