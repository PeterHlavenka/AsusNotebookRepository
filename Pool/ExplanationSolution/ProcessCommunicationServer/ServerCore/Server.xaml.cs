using System.Diagnostics;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ServerCore
{
    /// <summary>
    /// Interaction logic for Server.xaml
    /// </summary>
    public partial class Server : Window
    {
        private readonly Communicator m_communicator;

        public Server()
        {
            InitializeComponent();
            
            LaunchDotNet48WpfApplication();
            
            m_communicator = new Communicator();
            
            StartHost();
        }

        private void LaunchDotNet48WpfApplication()
        {
            
            string pathToDotNet48WpfApplication = @"c:\Pool\Adwind_Kite\Adwind\Apps\Process_Communication\Client\bin\Debug\Client.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(pathToDotNet48WpfApplication);
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            Process.Start(startInfo);
        }
        
        private async void StartHost()
        {
            string[] args = new string[3];
            
            IHost host = Host.CreateDefaultBuilder(null)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton(m_communicator);// not needed
                })
                .Build();

            await host.RunAsync();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            m_communicator.OnOnSendMessage();
        }
    }
}