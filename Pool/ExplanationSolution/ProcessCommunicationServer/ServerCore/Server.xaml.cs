using System.Diagnostics;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServerCore;

/// <summary>
///     Interaction logic for Server.xaml
/// </summary>
public partial class Server : Window
{
    private readonly Communicator m_communicator;
    private IHost m_host;
    private Process? m_pricingProcess;

    public Server()
    {
        InitializeComponent();

        m_communicator = new Communicator();

        StartHost();
    }

    private async void StartHost()
    {
        var args = new string[3];

        m_host = Host.CreateDefaultBuilder(null)
            .ConfigureLogging((context, builder) => builder.AddConsole())
            .ConfigureServices(services =>
            {
                // services.AddHostedService<ExternalPricingService>();
                services.AddHostedService<Worker>();
                services.AddSingleton(m_communicator); // not needed
            })
            .Build();

        var startAsyncTask = m_host.StartAsync();
        await startAsyncTask; // Wait for the host to start and services to start

        // var pricingService = m_host.Services.GetService<ExternalPricingService>(); // Retrieve the ExternalPricingService from the container


        // does not work ser is null
        var ser = m_host.Services.GetService<ExternalPricingService>();
        if (ser is not null) // still null
            await ser.StartAsync(CancellationToken.None);
    }

    private void SendMessage_OnClick(object sender, RoutedEventArgs e)
    {
        m_communicator.OnOnSendMessage();
    }

    private void StopPricing_OnClick(object sender, RoutedEventArgs e)
    {
        // var ser = m_host.Services.GetService<ExternalPricingService>();  
        // if (ser is not null)
        //     await ser.StopAsync(CancellationToken.None);

        m_pricingProcess?.CloseMainWindow(); //
        m_pricingProcess?.Kill();
        m_pricingProcess = null;
    }

    private void LaunchDotNet48WpfApplication(object sender, RoutedEventArgs e)
    {
        if (m_pricingProcess is not null) return;

        var pathToDotNet48WpfApplication = @"d:\AsusNotebookRepository\Pool\ExplanationSolution\ProcessCommunicationClient\Client\bin\Debug\Client.exe";
        var startInfo = new ProcessStartInfo(pathToDotNet48WpfApplication)
        {
            UseShellExecute = true,
            Verb = "runas"
        };
        m_pricingProcess = Process.Start(startInfo);
        // pricing?.CloseMainWindow();
    }
}