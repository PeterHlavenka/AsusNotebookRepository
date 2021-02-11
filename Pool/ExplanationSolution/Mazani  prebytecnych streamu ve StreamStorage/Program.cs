using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace Mazani__prebytecnych_streamu_ve_StreamStorage
{
    internal class Program
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType?.Name);

        private static void Main(string[] args)
        {
            var path = "d:\\Cache\\MIR.Media.Catching\\StreamStorage\\";

            var directory = Directory.CreateDirectory(path);

            var filesToRemove = directory.EnumerateFiles().OrderByDescending(d => d.LastWriteTime).Skip(3).ToList();

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