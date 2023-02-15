using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServerCore;

public class Worker : BackgroundService
{

    private NamedPipeServerStream m_pipeServer;
    private StreamWriter m_sw;

    public Worker(Communicator communicator)
    {
    
        
        communicator.OnSendMessage += SendMessage;
    }

    private async void SendMessage(object? sender, EventArgs e)
    {
        try
        {
            // Read user input and send that to the client process.
            m_sw = new StreamWriter(m_pipeServer);
            m_sw.AutoFlush = true;
           
            var nce = new Random().Next();
            await m_sw.WriteLineAsync("Testg"+nce);
        }
        // Catch the IOException that is raised if the pipe is broken
        // or disconnected.
        catch (IOException exc)
        {
            throw;
        }
    }

    public async Task Execute()
    {
        m_pipeServer = new NamedPipeServerStream("testPipe", PipeDirection.Out);
        // while (true)
        // {
            // await using
            // Wait for a client to connect
            await m_pipeServer.WaitForConnectionAsync();
            await Task.Delay(TimeSpan.FromHours(10));
            // await Task.Delay(100000, stoppingToken);  // Pipa se sama closne
        // }
        // return ExecuteAsync(new CancellationToken());
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // if (m_pipeServer.IsConnected)
        // {
        //     // m_pipeServer.Disconnect();
        //     return;
        // }
        
        

    }

    public void CloseConnection()
    {
        if (m_pipeServer is { IsConnected: true })
            m_pipeServer.Disconnect();

        m_pipeServer?.DisposeAsync();
    }

    public override void Dispose()
    {
        base.Dispose();
        m_pipeServer.Close();
        m_pipeServer.Disconnect();
        m_pipeServer.Dispose();
    }
}