﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreAppKite;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    
    public async Task Execute()
    {
        await ExecuteAsync(new CancellationToken());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await using var pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.Out);
            _logger.LogInformation("NamedPipeServerStream object created.");

            // Wait for a client to connect
            _logger.LogInformation("Waiting for client connection...");
            await pipeServer.WaitForConnectionAsync();

            _logger.LogInformation("Client connected.");
            try
            {
                // Read user input and send that to the client process.
                await using var sw = new StreamWriter(pipeServer);
                sw.AutoFlush = true;
                _logger.LogInformation("Enter text: ");
                sw.WriteLine("Testg");
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                _logger.LogInformation("ERROR: {0}", e.Message);
            }
            
            await Task.Delay(10000, stoppingToken);
        }
    }
}