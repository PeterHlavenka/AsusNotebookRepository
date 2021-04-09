using System;
using System.Diagnostics;

namespace Bat_runner
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            int maxStreams;
            string path;


            try
            {
                Process.Start("d:\\Cache\\batchfilename.bat");

                // Process externalProcess = new Process {StartInfo = {FileName = "d:\\Cache\\batchfilename.bat"}};
                // externalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                // externalProcess.Start();
                // externalProcess.WaitForExit();

            }
            catch (Exception e)
            {
                Console.WriteLine();
            }
        }
    }
}