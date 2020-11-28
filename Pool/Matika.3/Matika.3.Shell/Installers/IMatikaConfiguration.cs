using System;

namespace Matika._3.Shell.Installers
{
    public interface IMatikaConfiguration
    {
        string Info { get; }

        string IconPath { get; }

        TimeSpan DeploymentServiceCheckTimeSpan { get; }

        TimeSpan DeplymentServiceShutdownTimeSpan { get; }
    }
}