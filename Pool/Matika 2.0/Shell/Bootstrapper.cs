using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Matika.Gui;
using Shell.Installers;


namespace Shell
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        private IWindsorContainer m_container;

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetAssembly(typeof(MainViewModel))};
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            m_container = new WindsorContainer();
            m_container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());
            var manager = m_container.Resolve<IWindowManager>();
            var splashScreen = new SplashScreenViewModel();
            manager.ShowWindow(splashScreen);
            
            try
            {
                m_container.Install(new ShellInstaller());
                var root = m_container.Resolve<MainViewModel>();
                manager.ShowWindow(root);
                Application.Current.MainWindow = (Window) root.GetView();
                splashScreen.TryClose();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}