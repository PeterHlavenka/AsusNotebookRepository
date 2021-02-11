using System;
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
            var path = "d:\\Cache\\MIR.Media.Catching\\StreamStorage\\";

            if (Directory.Exists(path))
            {
                var directory = Directory.CreateDirectory(path);

                var filesToDelete = directory.EnumerateFiles().OrderByDescending(d => d.LastWriteTime).Skip(3).ToList();

                foreach (var fileInfo in filesToDelete)
                    try
                    {
                        m_log.Debug($@"Deleting file: {fileInfo.FullName}");
                        fileInfo.Delete();
                    }
                    catch (Exception e)
                    {
                        m_log.Debug($"Unable to delete temporary file {fileInfo.FullName}!", e);
                    }
            }
        }
    }
}