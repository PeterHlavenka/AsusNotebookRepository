using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using log4net;
using Microsoft.Extensions.Hosting;

namespace Client48
{
    public class MessageListener : BackgroundService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType?.Name);
        private readonly TextBox m_textBox;

        public MessageListener(TextBox clientTextBox)
        {
            m_textBox = clientTextBox;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                m_log.DebugFormat(@"Worker running at: {0}", DateTimeOffset.Now);

                using var pipeClient = new NamedPipeClientStream(".", "stringPipe", PipeDirection.In);

                // Connect to the pipe or wait until the pipe is available.
                m_log.Debug("Attempting to connect to pipe...");
                await pipeClient.ConnectAsync(stoppingToken);

                m_log.Debug("Connected to pipe.");

                using var sr = new StreamReader(pipeClient);
                while (await sr.ReadLineAsync() is { } temp)
                {
                    m_textBox.Text = temp;
                    m_log.DebugFormat("Received from server: {0}", temp);
                }
            }
        }
    }
}