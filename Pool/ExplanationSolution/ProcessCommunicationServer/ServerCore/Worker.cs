﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

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
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("testPipe", PipeDirection.Out);

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