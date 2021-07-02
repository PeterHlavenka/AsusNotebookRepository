using System.Threading.Tasks;
using TimeTrackerTutorial.PageModels;
using Xamarin.Forms;

namespace TimeTrackerTutorial.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync<TPageModel>(object navigationData = null, bool setRoot = false) where TPageModel : PageModelBase
        {
            var page = PageModelLocator.CreatePageFor(typeof(TPageModel));

            if (setRoot)
            {
                if (page is TabbedPage tabbedPage)
                {
                    Application.Current.MainPage = tabbedPage;
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
                
               
            }
            else
            {
                if (page is TabbedPage tabPage)
                {
                    Application.Current.MainPage = tabPage;
                }
                else if (Application.Current.MainPage is NavigationPage navPage)
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