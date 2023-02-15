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
    private readonly ILogger<Worker> m_logger;
    private NamedPipeServerStream m_pipeServer;
    private StreamWriter m_sw;

    public Worker(ILogger<Worker> logger, Communicator communicator)
    {
        m_logger = logger;

        communicator.OnSendMessage += SendMessage;
    }

    private async void SendMessage(object? sender, EventArgs e)
    {
        try
        {
            // Read user input and send that to the client process.
            m_sw = new StreamWriter(m_pipeServer);
            m_sw.AutoFlush = true;
            m_logger.LogInformation("Enter text: ");
            var nce = new Random().Next();
            await m_sw.WriteLineAsync("Testg"+nce);
        }
        // Catch the IOException that is raised if the pipe is broken
        // or disconnected.
        catch (IOException exc)
        { 
            m_logger.LogInformation("ERROR: {0}", exc.Message);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            m_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // await using
                m_pipeServer = new NamedPipeServerStream("testPipe", PipeDirection.Out);
            m_logger.LogInformation("NamedPipeServerStream object created.");

            // Wait for a client to connect
            m_logger.LogInformation("Waiting for client connection...");
            await m_pipeServer.WaitForConnectionAsync(stoppingToken);

            m_logger.LogInformation("Client connected.");

            await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
            // await Task.Delay(100000, stoppingToken);  // Pipa se sama closne
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        m_pipeServer.Close();
        m_pipeServer.Disconnect();
        m_pipeServer.Dispose();
    }
}