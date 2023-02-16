using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
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

        StartHost();

        VytvorPipuACekejNaSpojeni(new CancellationToken());
    }

    private async Task VytvorPipuACekejNaSpojeni(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("ObjectPipe", PipeDirection.Out);

        await m_pipeServer.WaitForConnectionAsync(stoppingToken);
        await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
    }

    private async void StartHost()
    {
        ClientTextBox.Text = "Starting";

        var host = Host.CreateDefaultBuilder(null)
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
                services.AddSingleton(ClientTextBox);
            })
            .Build();

        await host.RunAsync();
    }

    private void SendObject_OnClick(object sender, RoutedEventArgs e)
    {
        // create a new instance of MyObject
        CommonObject obj = new CommonObject { Id = new Random().Next(), Name = "John" };

        // serialize the object into a JSON string
        string jsonString = JsonSerializer.Serialize(obj);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(jsonString);

        //
        // // open the named pipe and send the byte array
        // using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "ObjectPipe", PipeDirection.Out);
        // pipeClient.Connect();
        // pipeClient.Write(buffer, 0, buffer.Length);
        // pipeClient.Flush();
        
        if (!m_pipeServer.IsConnected) { return; }
        // try
        // {
            m_pipeServer.Write(buffer, 0, buffer.Length);
            m_pipeServer.Flush();
            // Read user input and send that to the client process.
            // m_sw = new StreamWriter(m_pipeServer);
            // m_sw.AutoFlush = true;
            //
            // var nce = new Random().Next();
            // await m_sw.WriteLineAsync(nce.ToString());
        // }
    }
}