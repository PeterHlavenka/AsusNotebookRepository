using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using CommonLibs.XSerialization;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client48;

/// <summary>
///     Interaction logic for Client.xaml
/// </summary>
public partial class Client
{
    private IHost m_host;
    private List<IHostedService> m_hostedServices;
    private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType?.Name);

    public Client()
    {
        InitializeComponent();

        XSerializator.RegisterAssembly(Assembly.GetAssembly(typeof(CommonObject)), true);
        CreateHost();
    }

    private MessageListener MessageListener => m_hostedServices.OfType<MessageListener>().Single();
    private ObjectSender ObjectSender => m_hostedServices.OfType<ObjectSender>().Single();

    private async void CreateHost()
    {
        m_log.Debug($@" Creating host");
        m_host = Host.CreateDefaultBuilder(null)
            .ConfigureServices(services =>
            {
                services.AddHostedService<MessageListener>();
                services.AddHostedService<ObjectSender>();
                services.AddSingleton(ClientTextBox);
                services.AddHostedService<ObjectReceiver>();
            })
            .Build();

        m_hostedServices = m_host.Services.GetServices<IHostedService>().ToList();
        
        ClientTextBox.Text = "Host created";
        await m_host.RunAsync();  // runs all registered 
    }

    private void SendObject_OnClick(object sender, RoutedEventArgs e)
    {
        ObjectSender.SendObject();
    }

    private void Client_OnClosing(object sender, CancelEventArgs e)
    {
        m_host.StopAsync();
        m_host.Dispose();
    }
}