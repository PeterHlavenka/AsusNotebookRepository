using System;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Extensions.Hosting;
using Pricing.Core;

namespace ServerCore;

public class ObjectReceiver : BackgroundService
{
    private readonly TextBox m_serverTextBox;

    public ObjectReceiver(TextBox serverTextBox)
    {
        m_serverTextBox = serverTextBox;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pipeClient = new NamedPipeClientStream(".", "ObjectPipe", PipeDirection.In);
        while (true)
        {
            // Connect to the pipe or wait until the pipe is available.
            if (!pipeClient.IsConnected)
                await pipeClient.ConnectAsync(new CancellationToken());

            try
            {
                var buffer = new byte[30000];
                var read = await pipeClient.ReadAsync(buffer, 0, buffer.Length);

                var jsonString2 = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                var obj = JsonSerializer.Deserialize<PriceList>(jsonString2);

                m_serverTextBox.Text = obj?.Name + " "+ new Random().Next();
            }
            catch (Exception exception) // kdyz se pipa zavre a JsonSerializer je v pulce procesu
            {
                Console.WriteLine(exception);
            }
        }
    }
}