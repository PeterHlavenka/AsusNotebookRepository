using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Castle.Windsor;
using Matika;
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

            m_container.Install(new ShellInstaller());

            var root = m_container.Resolve<MainViewModel>();
            var manager = m_container.Resolve<IWindowManager>();
            manager.ShowDialog(root);
            Application.Shutdown();
        }
    }
}