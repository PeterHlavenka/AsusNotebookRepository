using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ClientTextBox.Text = "After initialize";
            
            StartHost();
        }
        
        private async void StartHost()
        {
            string[] args = new string[3];
            ClientTextBox.Text = "Starting";
            
            IHost host = Host.CreateDefaultBuilder(null)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton(ClientTextBox);
                })
                .Build();

            await host.RunAsync();
        }
    }
}