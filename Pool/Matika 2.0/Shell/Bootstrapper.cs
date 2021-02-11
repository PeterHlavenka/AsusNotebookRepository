using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Castle.Windsor;
using Matika.Gui;
using Shell.Installers;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;


namespace Shell
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        private IWindsorContainer m_container;
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType?.Name);

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetAssembly(typeof(MainViewModel))};
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {           
            m_log.Debug($@"Starting Matika at {DateTime.Now}");
            m_container = new WindsorContainer();

            m_container.Install(new ShellInstaller());

            var root = m_container.Resolve<MainViewModel>();
            var manager = m_container.Resolve<IWindowManager>();
            manager.ShowDialog(root);
           // Application.Shutdown();
        }
    }
}