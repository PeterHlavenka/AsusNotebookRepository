using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ServerCore;

public class ExternalPricingService : BackgroundService
{
    private Process? m_pricingProcess;

    /// <summary>
    /// Will be invoked when Host started;
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return LaunchDotNet48WpfApplication();
    }

    private Task LaunchDotNet48WpfApplication()
    {
        var pathToDotNet48WpfApplication = @"d:\AsusNotebookRepository\Pool\ExplanationSolution\ProcessCommunicationClient\Client\bin\Debug\Client.exe";
        var startInfo = new ProcessStartInfo(pathToDotNet48WpfApplication)
        {
            UseShellExecute = true,
            Verb = "runas"
        };
        m_pricingProcess = Process.Start(startInfo);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        m_pricingProcess?.CloseMainWindow();
        await base.StopAsync(cancellationToken);
    }

    public Task Execute()
    {
        return ExecuteAsync(new CancellationToken());
    }
}