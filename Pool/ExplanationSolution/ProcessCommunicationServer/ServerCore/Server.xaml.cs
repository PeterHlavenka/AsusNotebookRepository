using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
    private List<IHostedService> m_hostedServices;
    private IHost m_host;

    public Server()
    {
        InitializeComponent();
        DataContext = this;
        m_communicator = new Communicator();

        CreateHost();
    }

    private PipeSender PipeSender => m_hostedServices.OfType<PipeSender>().Single();
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
                services.AddHostedService<PipeSender>();
                services.AddHostedService<ExternalPricingService>();
                services.AddSingleton(m_communicator); // pro dependency injection
            })
            .Build();

        m_hostedServices = m_host.Services.GetServices<IHostedService>().ToList();
    }

    private async void OpenPricing_OnClick(object sender, RoutedEventArgs e)
    {
        if (PricingService.IsOpen) return;

        // start of all registered services doesnt work - second service will be cancelled by same token
        // await m_host.StartAsync(CancellationToken.None);  
        await PricingService.StartAsync(new CancellationToken()); // start of just one service
        await PipeSender.StartAsync(new CancellationToken());

        var pipeClient = new NamedPipeClientStream(".", "ObjectPipe", PipeDirection.In);
        while (true)
        {
            // Connect to the pipe or wait until the pipe is available.
            if (!pipeClient.IsConnected)
                await pipeClient.ConnectAsync(new CancellationToken());

            try
            {
                var buffer = new byte[1024];
                var read = await pipeClient.ReadAsync(buffer, 0, buffer.Length);

                var jsonString2 = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                var obj = JsonSerializer.Deserialize<CommonObject>(jsonString2);

                ServerTextBox.Text = obj?.Id.ToString();
            }
            catch (Exception exception) // kdyz se pipa zavre a JsonSerializer je v pulce procesu
            {
                Console.WriteLine(exception);
            }
        }
    }

    /// <summary>
    ///     Invokne event na tride Communicator. Tento event posloucha ExternalPricingService,
    /// </summary>
    private void SendMessage_OnClick(object sender, RoutedEventArgs e)
    {
        if (!PricingService.IsOpen) return;
        PipeSender.SendMessage(this, EventArgs.Empty);
        // m_communicator.OnOnSendMessage(); // stejne tak by to slo zavolat na instanci 
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
        PipeSender?.StopAsync(CancellationToken.None); // stop just one service
        PricingService?.StopAsync(CancellationToken.None);
    }

    private void Server_OnClosing(object? sender, CancelEventArgs e)
    {
        ClosePricingWindow();

        m_host.StopAsync();
        m_host.Dispose();
    }
}