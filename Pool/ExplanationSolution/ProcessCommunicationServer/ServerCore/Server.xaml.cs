using System.Diagnostics;
using System.Threading;
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
            
            // LaunchDotNet48WpfApplication();
            
            m_communicator = new Communicator();
            
            StartHost();
        }


        
        private async void StartHost()
        {
            string[] args = new string[3];
            
            IHost host = Host.CreateDefaultBuilder(null)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<ExternalPricingService>();
                    services.AddHostedService<Worker>();
                    services.AddSingleton(m_communicator);// not needed
                })
                .Build();

            await host.RunAsync();  // run all services
            
            // var ser = host.Services.GetService<ExternalPricingService>();  // run just one service
            // if (ser is not null)
            //     await ser.StartAsync(CancellationToken.None);
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            m_communicator.OnOnSendMessage();
        }
    }
}