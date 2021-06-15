using System.Windows.Input;
using TimeTrackerTutorial.PageModels.Base;
using TimeTrackerTutorial.Services.Navigation;
using Xamarin.Forms;

namespace TimeTrackerTutorial.PageModels
{
    public class LoginPageModel : PageModelBase
    {
        private readonly INavigationService m_navigationService;
        private ICommand m_signInCommand;

        public LoginPageModel(INavigationService navigationService)
        {
            m_navigationService = navigationService;
            SignInCommand = new Command(OnSignInAction);
        }

        private void OnSignInAction(object obj)
        {
            m_navigationService.NavigateToAsync<DashboardPageModel>();
        }

        public ICommand SignInCommand
        {
            get => m_signInCommand;
            set => SetProperty(ref m_signInCommand, value);
        }
    }
}