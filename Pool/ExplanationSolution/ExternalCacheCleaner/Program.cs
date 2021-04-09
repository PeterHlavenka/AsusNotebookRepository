using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace ExtendedCacheCleaner
{
    internal class Program
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType?.Name);

        private static void Main(string[] args)
        {
            int maxStreams = 3000;
            
            if (args.Length > 0 && int.TryParse(args[0], out var number))
            {
                maxStreams = number;
            }
            
            var path = "d:\\Cache\\MIR.Media.Catching\\StreamStorage\\";

            var directory = Directory.CreateDirectory(path);

            var filesToRemove = directory.EnumerateFiles().OrderByDescending(d => d.LastWriteTime).Skip(maxStreams).ToList();

            try
            {
                foreach (var fileInfo in filesToRemove)
                {
                    m_log.Debug($@"Deleting file: {fileInfo.FullName}");
                    fileInfo.Delete();
                }
            }
            catch (Exception e)
            {
                m_log.Error(@"Unable to clean cache", e);
            }
        }
    }
}