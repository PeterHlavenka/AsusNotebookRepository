using System.ComponentModel;
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
    private Worker m_pipeMesasageSender;

    public Server()
    {
        InitializeComponent();
        m_communicator = new Communicator();
        m_pipeMesasageSender = new Worker(m_communicator);
        CreateHost();
    }


    /// <summary>
    ///     Nastartuje host (pokud nebezi) a spusti registrovane servicy.
    /// </summary>
    private async void CreateHost()
    {
        m_host = Host.CreateDefaultBuilder(null)
            .ConfigureLogging((context, builder) => builder.AddConsole())
            .ConfigureServices(services =>
            {
               
                // services.AddSingleton<ExternalPricingService>(p => p.GetRequiredService<ExternalPricingService>());
                // services.AddSingleton<IHostedService>(p => p.GetRequiredService<ExternalPricingService>());
                //services.AddHostedService<Worker>();
                services.AddHostedService<ExternalPricingService>();// cannot retrieve instance of service
                //services.AddSingleton(m_communicator); // pro dependency injection
            })
            .Build();

        // await m_host.StartAsync();
    }
    
    
    
    private async void StartPricing_OnClick(object sender, RoutedEventArgs e)
    {
        // Check if the host is running
        // var appLifetime = m_host?.Services.GetRequiredService<IHostApplicationLifetime>();
        // if (appLifetime is { ApplicationStarted.IsCancellationRequested: false }) return;


        // var worker = new Worker(m_communicator);
        // await worker.StartAsync(new CancellationToken());



        
        var pricingService = m_host.Services.GetService<IHostedService>();
        if (pricingService is null) return;
        
        await pricingService.StartAsync(new CancellationToken());
        await m_pipeMesasageSender.Execute();
    }
    
    /// <summary>
    ///     Spusti StopAsync pro vsechny zaregistrovane IHostedService (Pricing, Pipe..)
    /// </summary>
    private void StopPricing_OnClick(object sender, RoutedEventArgs e)
    {
        ClosePricing(); 
    }

    private void ClosePricing()
    {
        m_pipeMesasageSender.CloseConnection();
        var pricingService = m_host.Services.GetService<IHostedService>();
        pricingService?.StopAsync(new CancellationToken());
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
        ClosePricing();
        
        m_host.StopAsync();
        m_host.Dispose();
    }
}