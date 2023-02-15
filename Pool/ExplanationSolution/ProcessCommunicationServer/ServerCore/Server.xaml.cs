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
    private ExternalPricingService m_externalPricingService;
    private Worker m_pipeMesasageSender;

    public Server()
    {
        InitializeComponent();
        m_communicator = new Communicator();
VyjebanaRobota();
        // CreateHost();
    }

    private async void VyjebanaRobota()
    {
        m_externalPricingService = new ExternalPricingService();
        m_pipeMesasageSender = new Worker(m_communicator);

        //await m_pipeMesasageSender.Execute();  // startuju pipu
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
                services.AddHostedService<Worker>();
                services.AddHostedService<ExternalPricingService>();// cannot retrieve instance of service
                services.AddSingleton(m_communicator); // pro dependency injection
            })
            .Build();

        await m_host.StartAsync();
    }
    
    
    
    private async void StartPricing_OnClick(object sender, RoutedEventArgs e)
    {
        // Check if the host is running
        // var appLifetime = m_host?.Services.GetRequiredService<IHostApplicationLifetime>();
        // if (appLifetime is { ApplicationStarted.IsCancellationRequested: false }) return;


        // var worker = new Worker(m_communicator);
        // await worker.StartAsync(new CancellationToken());


        await m_externalPricingService.Execute();
        await m_pipeMesasageSender.Execute();
        // var pricingService = m_host.Services.GetService<IHostedService>();
        // pricingService?.StartAsync(new CancellationToken());
    }
    
    /// <summary>
    ///     Spusti StopAsync pro vsechny zaregistrovane IHostedService (Pricing, Pipe..)
    /// </summary>
    private void StopPricing_OnClick(object sender, RoutedEventArgs e)
    {
        m_externalPricingService?.StopAsync(new CancellationToken());
        m_pipeMesasageSender.CloseConnection();
        // var test = m_host.Services.GetService<IHostedService>();
        // test?.StopAsync(new CancellationToken());
        //m_host.StopAsync();
        // m_host.Dispose();
    }
    
    /// <summary>
    ///     Invokne event na tride Communicator. Tento event posloucha ExternalPricingService,
    /// </summary>
    private void SendMessage_OnClick(object sender, RoutedEventArgs e)
    {
        m_communicator.OnOnSendMessage();
    }
}