using System;
using System.IO;
using Mediaresearch.Framework.Utilities.Threading.TimedProcessor;

namespace Mediaresearch.Framework.Utilities.IO
{
    public class OldFilesReaper : TimedProcessorBase
    {
        private readonly string m_directoryPath;
        private readonly TimeSpan m_maxFileAge;

        public OldFilesReaper(string directoryPath, TimeSpan maxFileAge, TimeSpan doWorkInterval, TimeSpan firstDoWorkInterval)
            : base(doWorkInterval, firstDoWorkInterval)
        {
            m_directoryPath = directoryPath;
            m_maxFileAge = maxFileAge;
        }

        protected override void DoWork()
        {
            if (!Directory.Exists(m_directoryPath))
                return;

            var directory = new DirectoryInfo(m_directoryPath);
            foreach (var fileInfo in directory.GetFiles())
            {
                try
                {
                    if (fileInfo.LastWriteTime <= DateTime.Now.Add(-m_maxFileAge))
                    {
                        fileInfo.Delete();
                    }
                }
                catch (Exception ex)
                {
                    if (m_log.IsErrorEnabled)
                        m_log.Error($"Exception occured during deleting {fileInfo.FullName}", ex);
                }
            }
        }
    }
}