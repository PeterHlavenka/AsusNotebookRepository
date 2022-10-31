using System.Threading.Tasks;

namespace TimeTrackerTutorial.PageModels
{
    public class DashboardPageModel : PageModelBase
    {
        private SettingsPageModel m_settingsPageModel;
        private TimeClockPageModel m_timeClockPageModel;
        private SummaryPageModel m_summaryPageModel;
        private ProfilePageModel m_profilePageModel;
        

        public TimeClockPageModel TimeClockPageModel
        {
            get => m_timeClockPageModel;
            set => SetProperty(ref m_timeClockPageModel, value);
        }

        public SummaryPageModel SummaryPageModel
        {
            get => m_summaryPageModel;
            set => SetProperty(ref m_summaryPageModel, value);
        }

        public ProfilePageModel ProfilePageModel
        {
            get => m_profilePageModel;
            set => SetProperty(ref m_profilePageModel, value);
        }

        public SettingsPageModel SettingsPageModel
        {
            get => m_settingsPageModel;
            set => SetProperty(ref m_settingsPageModel, value);
        }

        public DashboardPageModel(SettingsPageModel settingsPageModel, TimeClockPageModel timeClockPageModel, SummaryPageModel summaryPageModel, ProfilePageModel profilePageModel)
        {
            SettingsPageModel = settingsPageModel;
            TimeClockPageModel = timeClockPageModel;
            SummaryPageModel = summaryPageModel;
            ProfilePageModel = profilePageModel;
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAny(base.InitializeAsync(navigationData),
                ProfilePageModel.InitializeAsync(null),
                SettingsPageModel.InitializeAsync(null),
                SummaryPageModel.InitializeAsync(null),
                TimeClockPageModel.InitializeAsync(null)
               );
        }
    }
}