using System;
using System.Collections.Generic;
using TimeTrackerTutorial.Pages;
using TimeTrackerTutorial.Services.Navigation;
using TinyIoC;
using Xamarin.Forms;

namespace TimeTrackerTutorial.PageModels.Base
{
    public class PageModelLocator
    {
        private static TinyIoCContainer _container;
        private static Dictionary<Type, Type> _viewLookup;

        public PageModelLocator()
        {
            _container = new TinyIoCContainer();
            _viewLookup = new Dictionary<Type, Type>();
            
            // Register pages and page models
            Register<LoginPageModel, LoginPage>();
            Register<DashboardPageModel, DashboardPage>();
            Register<ProfilePageModel, ProfilePage>();
            Register<SettingsPageModel, SettingsPage>();
            Register<SummaryPageModel, SummaryPage>();
            Register<TimeClockPageModel, TimeClockPage>();

            // Register services (services are registered as Singletons default)
            _container.Register<INavigationService, NavigationService>();

        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        public static Page CreatePageFor(Type pageModelType)
        {
            var pageType = _viewLookup[pageModelType];
            var page = (Page) Activator.CreateInstance(pageType);
            var pageModel = _container.Resolve(pageModelType);
            page.BindingContext = pageModel;
            return page;
        }

        static void Register<TPageModel, TPage>() where TPageModel : PageModelBase where TPage : Page
        {
            _viewLookup.Add(typeof(TPageModel), typeof(TPage));
            _container.Register<TPageModel>();
        }
    }
}