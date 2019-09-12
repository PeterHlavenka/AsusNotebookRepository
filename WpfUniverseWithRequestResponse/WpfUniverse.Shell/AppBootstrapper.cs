using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Castle.DynamicProxy;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using WpfUniverse.Common;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Requests;
using WpfUniverse.Common.Responses;
using WpfUniverse.Core;
using WpfUniverse.Core.ServiceActions;
using WpfUniverse.Gui.ViewModels;
using WpfUniverse.Shell.Installers;
using Component = Castle.MicroKernel.Registration.Component;
using CoreInstaller = WpfUniverse.Core.Installers.CoreInstaller;

namespace WpfUniverse.Shell
{
    public class AppBootstrapper : Bootstrapper<MainViewModel>
    {
        protected override void Configure()
        {
            ViewLocator.LocateForModel = (model, displayLocation, context) =>
            {
                var unproxiedModelType = ProxyUtil.GetUnproxiedType(model);
                return ViewLocator.LocateForModelType(unproxiedModelType ?? model.GetType(), displayLocation, context);
            };
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var container = InitializeContainer();

            SetCulture();

            InitializeServerProxy(container);
            //SubscribeServiceActions(container);

            var windowManager = container.Resolve<IWindowManager>();
            var rootViewModel = container.Resolve<MainViewModel>();

            rootViewModel.Initialize();
            windowManager.ShowDialog(rootViewModel);
            Application.Shutdown();
        }

        private void SetCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("cs-CZ");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("cs-CZ");
        }

        //private void SubscribeServiceActions(IWindsorContainer container)
        //{
        //    IServiceActionSubscriber subscriber = container.Resolve<IServiceActionSubscriber>();

        //    subscriber.Subscribe<SelectAllGalaxiesRequest, SelectAllGalaxiesResponse>(typeof(SelectAllGalaxiesServiceAction));
        //    subscriber.Subscribe<SelectPlanetsRequest, SelectPlanetsResponse>(typeof(SelectPlanetsServiceAction));
        //    subscriber.Subscribe<DeletePlanetRequest, DoneResponse>(typeof(DeletePlanetServiceAction));
        //    subscriber.Subscribe<DeleteVlastnostsFromPlanetRequest, DeleteVlastnostsFromPlanetResponse>(typeof(DeleteVlastnostsFromPlanetServiceAction));
        //    subscriber.Subscribe<UpdatePlanetRequest, UpdatePlanetResponse>(typeof(UpdatePlanetServiceAction));
        //    subscriber.Subscribe<UpdateGalaxyRequest, UpdateGalaxyResponse>(typeof(UpdateGalaxyServiceAction));
        //    subscriber.Subscribe<InsertPlanetRequest, InsertPlanetResponse>(typeof(InsertPlanetServiceAction));
        //    subscriber.Subscribe<SelectPropertiesRequest, ListResponse<VlastnostDataContract>>(typeof(SelectPropertiesServiceAction));
        //    subscriber.Subscribe<GetCheckedRequest, GetCheckedResponse>(typeof(GetCheckedServiceAction));
        //    subscriber.Subscribe<SaveChangesRequest, DoneResponse>(typeof(SaveChangesServiceAction));
        //    subscriber.Subscribe<CheckAllPropertiesRequest, DoneResponse>(typeof(CheckAllPropertiesServiceAction));
        //    subscriber.Subscribe<UncheckAllPropertiesRequest, DoneResponse>(typeof(UncheckAllPropertiesServiceAction));
        //    subscriber.Subscribe<AddPropertyRequest, AddPropertyResponse>(typeof(AddPropertyServiceAction));
        //}

        // kde se maji hledat 
        private void InitializeServerProxy(IWindsorContainer container)
        {
            IRequestsServiceActionsAssemblyProvider provider = container.Resolve<IRequestsServiceActionsAssemblyProvider>();
            provider.RegisterAssemblies(Assembly.GetAssembly(typeof(WpfUniverseCommonAssemblyIdentificator)),
                Assembly.GetAssembly(typeof(WpfUniverseCoreAssemblyIdentificator)));
        }

        // kde se maji hledat view
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetAssembly(typeof(MainViewModel)) };
        }

        private IWindsorContainer InitializeContainer()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.Install(FromAssembly.Containing(typeof(Installer))); 
            container.Install(FromAssembly.Containing(typeof(CoreInstaller))); 
            container.Resolve<IDaoSource>();

            return container;
        }


    }

}