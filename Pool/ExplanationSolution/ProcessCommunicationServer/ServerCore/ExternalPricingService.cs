using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ServerCore;

public class ExternalPricingService : BackgroundService
{
    private Process? m_pricingProcess;

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
        // pricing?.CloseMainWindow();
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        m_pricingProcess?.CloseMainWindow();
        await base.StopAsync(cancellationToken);
    }
}