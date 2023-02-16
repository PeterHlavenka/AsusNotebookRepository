using System;
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

    public Server()
    {
        InitializeComponent();
        DataContext = this;
        m_communicator = new Communicator();

        CreateHost();
    }

    private MesasageSender MessageSender => m_hostedServices.OfType<MesasageSender>().Single();
    private ExternalPricingService PricingService => m_hostedServices.OfType<ExternalPricingService>().Single();
    private ObjectReceiver ObjectReceiver => m_hostedServices.OfType<ObjectReceiver>().Single();

    /// <summary>
    ///     Nastartuje host (pokud nebezi) a spusti registrovane servicy.
    /// </summary>
    private void CreateHost()
    {
        m_host = Host.CreateDefaultBuilder(null)
            .ConfigureLogging((context, builder) => builder.AddConsole())
            .ConfigureServices(services =>
            {
                services.AddHostedService<MesasageSender>();
                services.AddHostedService<ExternalPricingService>();
                services.AddHostedService<ObjectReceiver>();
                services.AddSingleton(m_communicator); // pro dependency injection
                services.AddSingleton(ServerTextBox);
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
        await MessageSender.StartAsync(new CancellationToken());
        await ObjectReceiver.StartAsync(new CancellationToken());
    }


    private void SendMessage_OnClick(object sender, RoutedEventArgs e)
    {
        if (!PricingService.IsOpen) return;
        MessageSender.SendMessage(this, EventArgs.Empty);
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
        // m_host.StopAsync(CancellationToken.None); // not possible durring cancellation
        MessageSender?.StopAsync(CancellationToken.None); // stop just one service
        PricingService?.StopAsync(CancellationToken.None);
        ObjectReceiver?.StopAsync(CancellationToken.None);
    }

    private void Server_OnClosing(object? sender, CancelEventArgs e)
    {
        ClosePricingWindow();

        m_host.StopAsync();
        m_host.Dispose();
    }
}