using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace log4net_explanation
{
    static class Program
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType?.Name);

        private static void Main(string[] args)
        {
            m_log.Debug($@"Starting cache maintenance for StreamStorage");
            Process externalProcess = new Process {StartInfo = {FileName = "CacheMaintenance/Mazani  prebytecnych streamu ve StreamStorage.exe"}};
            
            // nekdy je potreba opacny slash: 
            //Process externalProcess = new Process {StartInfo = {FileName = "CacheMaintenance\\Mazani  prebytecnych streamu ve StreamStorage.exe"}};
            //externalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            externalProcess.Start();
            externalProcess.WaitForExit();
        }
    }
}