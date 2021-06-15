using System.Threading.Tasks;
using TimeTrackerTutorial.PageModels;
using Xamarin.Forms;

namespace TimeTrackerTutorial.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync<TPageModelBase>(object navigationData = null, bool setRoot = false)
        {
            var page = PageModelLocator.CreatePageFor(typeof(TPageModelBase));

            if (setRoot)
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else
            {
                if (Application.Current.MainPage is NavigationPage navPage)
                {
                    await navPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
            }

            if (page.BindingContext is PageModelBase pmBase)
            {
                await pmBase.InitializeAsync(navigationData);
            }
            
            await Task.CompletedTask;
        }

        public Task GoBackAsync()
        {
            return Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}