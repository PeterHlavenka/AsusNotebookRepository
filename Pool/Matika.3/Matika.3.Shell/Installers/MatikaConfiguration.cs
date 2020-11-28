using System;
using System.IO;

namespace Matika._3.Shell.Installers
{
    public class MatikaConfiguration : IMatikaConfiguration
    {
        public MatikaConfiguration(string info, string iconPath, TimeSpan deploymentServiceCheckTimeSpan, TimeSpan deplymentServiceShutdownTimeSpan)
        {
            Info = info;
            IconPath = iconPath;
            DeploymentServiceCheckTimeSpan = deploymentServiceCheckTimeSpan;
            DeplymentServiceShutdownTimeSpan = deplymentServiceShutdownTimeSpan;
        }

        public static string TempFilesDirectory { get; } = Path.Combine(Path.GetTempPath(), "Matika");
        public string Info { get; }
        public string IconPath { get; }
        public TimeSpan DeploymentServiceCheckTimeSpan { get; }
        public TimeSpan DeplymentServiceShutdownTimeSpan { get; }
    }
}