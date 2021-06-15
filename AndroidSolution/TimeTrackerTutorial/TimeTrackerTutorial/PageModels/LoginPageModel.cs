using System.Windows.Input;
using TimeTrackerTutorial.Services.Account;
using TimeTrackerTutorial.Services.Navigation;
using Xamarin.Forms;

namespace TimeTrackerTutorial.PageModels
{
    public class LoginPageModel : PageModelBase
    {
        private readonly INavigationService m_navigationService;
        private readonly IAccountService m_accountService;
        private ICommand m_logInCommand;
        private string m_userName;
        private string m_password;

        public LoginPageModel(INavigationService navigationService, IAccountService accountService)
        {
            m_navigationService = navigationService;
            m_accountService = accountService;
            
            LogInCommand = new Command(DoLogInAction);
        }
        
        public ICommand LogInCommand
        {
            get => m_logInCommand;
            set => SetProperty(ref m_logInCommand, value);
        }

        public string UserName
        {
            get => m_userName;
            set => SetProperty(ref m_userName, value);
        }

        public string Password
        {
            get => m_password;
            set => SetProperty(ref m_password, value);
        }

        private async void DoLogInAction(object obj)
        {
            var loginAttempt = await m_accountService.LoginAsync(UserName, Password);

            if (loginAttempt)
            {
                await m_navigationService.NavigateToAsync<DashboardPageModel>();
            }
            else
            {
                //todo : Display an allert for failure!
            }
            
            
        }
    }
}