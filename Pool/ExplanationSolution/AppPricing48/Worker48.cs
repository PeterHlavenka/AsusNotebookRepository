using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppPricing48
{
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

                using var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.In);

                // Connect to the pipe or wait until the pipe is available.
                _logger.LogInformation("Attempting to connect to pipe...");
                pipeClient.Connect();

                _logger.LogInformation("Connected to pipe.");

                using var sr = new StreamReader(pipeClient);
                string? temp;
                while ((temp = sr.ReadLine()) != null)
                {
                    _logger.LogInformation("Received from server: {0}", temp);
                }
            
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}