using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using Common;
using Matika._3.Core;
using Matika._3.Gui;
using Matika._3.Shell.Installers;
using Matika._3.Shell.Subscribers;
using Mediaresearch.Framework.Communication.Common;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Mediaresearch.Framework.Mapping;
using Mediaresearch.Framework.Mapping.Castle;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Matika._3.Shell
{
    public class Bootstrapper : BootstrapperBase
    {
        private static readonly string m_applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private WindsorContainer m_globalContainer;

        public Bootstrapper()
        {
            Initialize();
        }

        // private void ConfigureUser()
        // {
        //     var userSource = m_globalContainer.Resolve<IUserSource>();
        //     var auditableIdentityProvider = m_globalContainer.Resolve<IAuditableIdentityProvider>();
        //
        //     var login = WindowsIdentity.GetCurrent().Name;
        //
        //     if (userSource.Login(login))
        //     {
        //         var userId = userSource.GetUserId();
        //         auditableIdentityProvider.SetAuditableIdentity(new AuditableIdentity(userId, login));
        //     }
        //     else
        //     {
        //         IsUserLogged = false;
        //     }            
        // }
        //
        // private void ConfigureAppEnvironment()
        // {
        //     var environment = m_globalContainer.Resolve<IEnvironmentSource>();
        //
        //     environment.SetAppEnvironment();
        // }

        private bool IsUserLogged { get; set; } = true;

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            ConfigureLog4Net();
            // ConfigureLanguage();

            m_globalContainer = new WindsorContainer(new XmlInterpreter(Path.Combine(m_applicationDirectory, "Matika.3.Shell.Container.config")));

            var changingConfiguration = m_globalContainer.Resolve<IMatikaConfiguration>();
            var info = changingConfiguration.Info;
            var iconPath = changingConfiguration.IconPath;

            MainViewModel rootViewModel = null;

            var configuration = m_globalContainer.Resolve<LocalhostDbConfiguration>();

            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = $"{GuiResources.Math} {fvi.FileVersion}";
            var appVersionInfo = $"{version} ({configuration.DefaultDataSource})";

            m_globalContainer.Install(new ShellInstaller());

            void InitAction()
            {
                m_globalContainer.Register(Component.For<IWindsorContainer>().Instance(m_globalContainer).LifestyleSingleton());
                m_globalContainer.Install(new LocalhostDbAccessInstaller());
                m_globalContainer.Install( FromAssembly.Containing<CoreAssemblyIdentificator>());


                m_globalContainer.Resolve<IDaoSource>();

                // var deploymentService = m_globalContainer.Resolve<IDeploymentService>();
                // var checkTimeSpan = changingConfiguration.DeploymentServiceCheckTimeSpan;
                // var shutDownTimeSpan = changingConfiguration.DeplymentServiceShutdownTimeSpan;
                // deploymentService.InitializeAutoUpdater(checkTimeSpan, shutDownTimeSpan, appVersionInfo);

                var mappingConfiguratior = new CastleDependencyMappingConfigurator(m_globalContainer, m_globalContainer.ResolveAll<MappingConfiguratorBase>());
                mappingConfiguratior.Configure();

                if (!Directory.Exists(MatikaConfiguration.TempFilesDirectory)) Directory.CreateDirectory(MatikaConfiguration.TempFilesDirectory);

                ConfigureSubscriber();
                // ConfigureUser();
                // ConfigureAppEnvironment();

                rootViewModel = m_globalContainer.Resolve<MainViewModel>();
                rootViewModel.AppVersionDescription = appVersionInfo;
            }

            var splashScreen = new SplashScreenViewModel(InitAction, version, iconPath, info);

            var windowManager = m_globalContainer.Resolve<IWindowManager>();
            windowManager.ShowDialog(splashScreen);

            // if (IsUserLogged)
            // {   
            try
            {
                windowManager.ShowDialog(rootViewModel);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
       
            // }
            // else
            // {
            //     var mb = new MessageBoxViewModel(GuiResources.UserDoesNotHaveRights, false, true, string.Empty);
            //     windowManager.ShowDialog(mb);
            // }

            //Application.Shutdown();

           //m_globalContainer.Dispose();
        }

        //kde se maji hledat View
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetAssembly(typeof(MainViewModel))};
        }

        // kde se maji hledat requesty a responsy
        private void ConfigureSubscriber()
        {
            var coreAssembly = typeof(CommonAssemblyIdentificator).Assembly;

            var notificationProvider = m_globalContainer.Resolve<INotificationsReceiversAssemblyProvider>();
            notificationProvider.RegisterAssemblies(coreAssembly, coreAssembly);

            var notificationsSubcriber = m_globalContainer.Resolve<INotificationReceiverSubscriber>();
            notificationsSubcriber.SubscribeAll();

            var provider = m_globalContainer.Resolve<IRequestsServiceActionsAssemblyProvider>();
            provider.RegisterAssemblies(coreAssembly, typeof(CoreAssemblyIdentificator).Assembly);

            var subscriber = m_globalContainer.Resolve<IServiceActionSubscriber>();
            subscriber.SubscribeAll();

            var customSubscriber = m_globalContainer.Resolve<CustomServiceActionSubscriber>();
            customSubscriber.Subscribe();
        }

        private static void ConfigureLog4Net()
        {
            var logConfigFile = new FileInfo(Path.Combine(m_applicationDirectory, "Matika.3.Shell.log4net"));

            //XmlConfigurator.Configure(logConfigFile);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log(e.IsTerminating, e.ExceptionObject, m_log);
        }

        public static void Log(bool isTerminating, object exceptionObject, ILog logger)
        {
            var message = $"Unhandled exception in application (IsTerminating = {isTerminating})";

            /*
             * Why is UnhandledExceptionEventArgs.ExceptionObject of type Object and not Exception?
             * While not all languages support throwing non-Exception type exceptions, the CLR and IL allow for throwing any Object.
             * In general, throwing non-Exception types is discouraged because most developers do not expect this to occur, 
             * and are not likely to catch the object. 
             * On the other hand, developers who are overriding the unhandled exception logic may need to catch non-Exception objects, also.
             */
            if (exceptionObject is Exception ex)
                logger.Fatal(message, ex);
            else
                logger.Fatal($"{message} : {exceptionObject}");
        }
    }
}