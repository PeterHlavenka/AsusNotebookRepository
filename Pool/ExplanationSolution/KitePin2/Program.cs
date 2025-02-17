using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KitePin2
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
            var correctStart = Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed")?.ToLower() == "true";
            if (!correctStart)
            {
                // var programData = System.IO.Path.Combine(KnownFolders.GetPath(KnownFolder.Programs), "");
                //var programData = System.IO.Path.Combine(KnownFolders.ProgramData.Path, "");
                var str = @"C:\Users\phlavenka\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\KitePin";

                var files = System.IO.Directory.GetFiles(str, "*.appref-ms");
                if (files.Length == 1)
                {
                   // MessageBox.Show("KitePin is not actualized correctly. Please, run the application from the Start Menu.");

                    try
                    {
                        var si = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = files[0],
                            UseShellExecute = true,
                            Verb = "open"
                        };
                        System.Diagnostics.Process.Start(si);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }

                    var app = new Application();
                    var mainWindow = new MainWindow();
                    app.Run(mainWindow);

                    return;
                }

                // throw new Exception("KitePin is not actualized correctly.");

            }
            //var app = new Application();
            //var mainWindow = new MainWindow();
            //app.Run(mainWindow);
        }
    }
}
