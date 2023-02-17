using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ServerCore;

public class ExternalPricingService : BackgroundService
{
    private Process? m_pricingProcess;

    /// <summary>
    /// This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return LaunchDotNet48WpfApplication();
    }

    public bool IsOpen => m_pricingProcess is { HasExited: false };
    
    private Task LaunchDotNet48WpfApplication()
    {
        var dir = Directory.GetCurrentDirectory();
        for (var i = 0; i < 5; i++)
        {
            if (dir != null) dir = Directory.GetParent(dir)?.FullName;
        }

        if (dir == null) { return Task.CompletedTask; }
        var pathToDotNet48WpfApplication = Path.Combine(dir, @"ProcessCommunicationClient\Client48\bin\Debug\Client48.exe");
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
}

// to asam
// z:\srendl\ProcessCommunicationServer\ServerCore\bin\Debug\net6.0-windows\ServerCore.exe