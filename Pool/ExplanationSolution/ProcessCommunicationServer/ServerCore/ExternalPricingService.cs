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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return LaunchDotNet48WpfApplication();
    }

    private Task LaunchDotNet48WpfApplication()
    {
        var pathToDotNet48WpfApplication = @"d:\AsusNotebookRepository\Pool\ExplanationSolution\ProcessCommunicationClient\Client\bin\Debug\Client.exe";
        //
        // var currentUser = WindowsIdentity.GetCurrent();
        // var trustStatus = new X509ChainPolicy();
        // trustStatus.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
        // trustStatus.RevocationFlag = X509RevocationFlag.EntireChain;
        // trustStatus.ExtraStore.Add(new X509Certificate2("C:\\MyCert.pfx"));
        // var chain = new X509Chain();
        // chain.ChainPolicy = trustStatus;
        // var isValid = chain.Build(new X509Certificate2(pathToDotNet48WpfApplication));
        // if (isValid)
        // {
        //     var rule = new System.Security.AccessControl.FileSystemAccessRule(
        //         currentUser.Name,
        //         System.Security.AccessControl.FileSystemRights.FullControl,
        //         System.Security.AccessControl.AccessControlType.Allow);
        //     var info = new System.IO.FileInfo("C:\\MyApp.exe");
        //     var security = info.GetAccessControl();
        //     security.AddAccessRule(rule);
        //     info.SetAccessControl(security);
        // }
        
        
        
        
        
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