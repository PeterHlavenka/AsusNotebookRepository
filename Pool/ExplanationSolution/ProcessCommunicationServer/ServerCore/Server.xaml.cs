using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServerCore;

/// <summary>
///     Interaction logic for Server.xaml
/// </summary>
public partial class Server
{
    private readonly Communicator m_communicator;
    private IHost m_host;
    private List<IHostedService> m_hostedServices;
    private bool m_pricingIsRunning;

    public Server()
    {
        InitializeComponent();
        m_communicator = new Communicator();
        
        CreateHost();
    }

    private Worker Worker => m_hostedServices.OfType<Worker>().Single();
    private ExternalPricingService PricingService => m_hostedServices.OfType<ExternalPricingService>().Single();

    /// <summary>
    ///     Nastartuje host (pokud nebezi) a spusti registrovane servicy.
    /// </summary>
    private void CreateHost()
    {
        m_host = Host.CreateDefaultBuilder(null)
            .ConfigureLogging((context, builder) => builder.AddConsole())
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
                services.AddHostedService<ExternalPricingService>();
                services.AddSingleton(m_communicator); // pro dependency injection
            })
            .Build();

        m_hostedServices = m_host.Services.GetServices<IHostedService>().ToList();
        
    }

    private async void OpenPricing_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (m_pricingIsRunning) return;

            m_pricingIsRunning = true;
            
            // start of all registered services doesnt work - second service will be cancelled by same token
            // await m_host.StartAsync(CancellationToken.None);  
            await PricingService.StartAsync(new CancellationToken()); // start of just one service
            await Worker.StartAsync(new CancellationToken());
        }
        finally
        {
            m_pricingIsRunning = false;
        }
    }

    /// <summary>
    ///     Spusti StopAsync pro vsechny zaregistrovane IHostedService (Pricing, Pipe..)
    /// </summary>
    private void ClosePricing_OnClick(object sender, RoutedEventArgs e)
    {
        ClosePricingWindow();
    }

    private void ClosePricingWindow()
    {
        Worker?.StopAsync(CancellationToken.None);  // stop just one service
        PricingService?.StopAsync(CancellationToken.None);
    }


    /// <summary>
    ///     Invokne event na tride Communicator. Tento event posloucha ExternalPricingService,
    /// </summary>
    private void SendMessage_OnClick(object sender, RoutedEventArgs e)
    {
        m_communicator.OnOnSendMessage();
    }

    private void Server_OnClosing(object? sender, CancelEventArgs e)
    {
        ClosePricingWindow();

        m_host.StopAsync();
        m_host.Dispose();
    }
}