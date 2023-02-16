using System;
using System.ComponentModel;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client48;

/// <summary>
///     Interaction logic for Client.xaml
/// </summary>
public partial class Client
{
    private NamedPipeServerStream m_pipeServer;

    public Client()
    {
        InitializeComponent();
        ClientTextBox.Text = "After initialize";

        CreateHost();

        CreatePipeAndWaitForConnection(new CancellationToken());
    }

    private async Task CreatePipeAndWaitForConnection(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("ObjectPipe", PipeDirection.Out);

        await m_pipeServer.WaitForConnectionAsync(stoppingToken);
        await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
    }

    private async void CreateHost()
    {
        ClientTextBox.Text = "Starting";

        var host = Host.CreateDefaultBuilder(null)
            .ConfigureServices(services =>
            {
                services.AddHostedService<MessageListener>();
                services.AddSingleton(ClientTextBox);
            })
            .Build();

        await host.RunAsync();  // runs all registered services
    }

    private void SendObject_OnClick(object sender, RoutedEventArgs e)
    {
        // create a new instance of MyObject
        var obj = new CommonObject { Id = new Random().Next() };

        // serialize the object into a JSON string
        var jsonString = JsonSerializer.Serialize(obj);
        var buffer = Encoding.UTF8.GetBytes(jsonString);
        
        if (!m_pipeServer.IsConnected) return;

        m_pipeServer.Write(buffer, 0, buffer.Length);
        m_pipeServer.Flush();
    }
}