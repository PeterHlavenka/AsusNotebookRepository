using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ServerCore;

public class MesasageSender : BackgroundService
{
    private NamedPipeServerStream m_pipeServer;
    private StreamWriter m_sw;

    public MesasageSender(Communicator communicator)
    {
        communicator.OnSendMessage += SendMessage; // not needed
    }

    public async void SendMessage(object? sender, EventArgs e)
    {
        if (!m_pipeServer.IsConnected) return;

        m_sw = new StreamWriter(m_pipeServer);
        m_sw.AutoFlush = true;

        var nce = new Random().Next();
        await m_sw.WriteLineAsync(nce.ToString());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("stringPipe", PipeDirection.Out);

        await m_pipeServer.WaitForConnectionAsync(stoppingToken);
        await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (m_pipeServer is { IsConnected: true })
            m_pipeServer.Disconnect();

        m_pipeServer?.Close();
        await base.StopAsync(cancellationToken);
    }
}