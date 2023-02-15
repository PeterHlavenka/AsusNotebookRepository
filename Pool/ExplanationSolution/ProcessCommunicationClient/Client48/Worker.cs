using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client48
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> m_logger;
        private readonly TextBox m_textBox;

        public Worker(ILogger<Worker> logger, TextBox clientTextBox)
        {
            m_logger = logger;
            m_textBox = clientTextBox;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                m_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var pipeClient = new NamedPipeClientStream(".", "testPipe", PipeDirection.In);

                // Connect to the pipe or wait until the pipe is available.
                m_logger.LogInformation("Attempting to connect to pipe...");
                await pipeClient.ConnectAsync(stoppingToken);

                m_logger.LogInformation("Connected to pipe.");

                using var sr = new StreamReader(pipeClient);
                while (await sr.ReadLineAsync() is { } temp)
                {
                    m_textBox.Text = temp;
                    m_logger.LogInformation("Received from server: {0}", temp);
                }
            
                // this

                // await Task.Delay(10000, stoppingToken);
            }
        }
    }
}