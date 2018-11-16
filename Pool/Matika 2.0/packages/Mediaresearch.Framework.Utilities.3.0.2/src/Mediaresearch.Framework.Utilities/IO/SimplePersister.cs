using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace Mediaresearch.Framework.Utilities.IO
{
    public class SimplePersister<TPersistType> : ISimplePersister<TPersistType>
        where TPersistType : class
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string m_directoryPath;
        private readonly object m_lock = new object();
        private Dictionary<string, TPersistType> m_cache;
        private bool m_loaded;

        public SimplePersister()
        {
            m_directoryPath = $"{typeof(TPersistType).Name}s";
        }

        public SimplePersister(string directoryPath)
        {
            m_directoryPath = directoryPath;
        }

        public Task PersistAsync(string identificator, TPersistType objectToStore)
        {
            return Task.Factory.StartNew(() =>
            {
                Persist(identificator, objectToStore);
            });
        }

        public void Persist(string identificator, TPersistType objectToStore)
        {
            lock (m_lock)
            {
                try
                {
                    m_log.Info($"Saving object {objectToStore} under indetificator {GetFileName(identificator)}; {objectToStore}");

                    var data = BinaryFormatterHelper.SerializeSingleObject(objectToStore);

                    using (var fs = new FileStream(GetFileName(identificator), FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }

                    if (!m_cache.ContainsKey(identificator))
                        m_cache.Add(identificator, objectToStore);

                    m_cache[identificator] = objectToStore;
                }
                catch (Exception ex)
                {
                    if (m_log.IsErrorEnabled)
                        m_log.Error($"Can't save {objectToStore} to {GetFileName(identificator)}", ex);
                }
            }
        }

        public bool FreeResources(string identificator)
        {
            var fi = new FileInfo(GetFileName(identificator));
            try
            {
                if (fi.Exists)
                    fi.Delete();

                m_cache.Remove(identificator);

                return true;
            }
            catch (Exception e)
            {
                if (m_log.IsErrorEnabled)
                    m_log.Error($"{typeof(SimplePersister<TPersistType>).Name}: Error occured during deleting {fi.FullName}", e);
                return false;
            }
        }

        public bool TryGetObjectFor(string identificator, out TPersistType obj)
        {
            if (!m_loaded)
                m_cache = LoadPersistedObjects();

            if (!m_cache.ContainsKey(identificator))
            {
                obj = null;
                return false;
            }

            obj = m_cache[identificator];
            return true;
        }

        public TPersistType GetObjectFor(string identificator)
        {
            if (!m_loaded)
                m_cache = LoadPersistedObjects();

            if (!m_cache.ContainsKey(identificator))
            {
                throw new ArgumentNullException(identificator, "There is  no object with such identificator stored here.");
            }

            return m_cache[identificator];
        }

        public Dictionary<string, TPersistType> LoadPersistedObjects()
        {
            var resultDict = new Dictionary<string, TPersistType>();

            var directory = new DirectoryInfo(m_directoryPath);
            if (!directory.Exists)
            {
                directory.Create();
                return resultDict;
            }

            try
            {
                foreach (var source in directory.GetFiles())
                {
                    try
                    {
                        byte[] data;
                        using (var fs = new FileStream(GetFileName(source.Name), FileMode.Open))
                        {
                            data = new byte[fs.Length];
                            fs.Read(data, 0, (int)fs.Length);
                        }

                        var deserialized = BinaryFormatterHelper.DeserializeSingleObject<TPersistType>(data);

                        resultDict.Add(source.Name, deserialized);
                    }
                    catch (Exception e)
                    {
                        if (m_log.IsErrorEnabled)
                            m_log.Error($"Can't load objects from file {source.FullName}. See inner exception for detailed info.", e);
                        continue;
                    }
                }

                return resultDict;
            }
            catch (Exception ex)
            {
                if (m_log.IsErrorEnabled)
                    m_log.Error($"Can't load files from {directory.FullName}. See inner exception for detailed info.", ex);
                return resultDict;
            }
            finally
            {
                m_loaded = true;
            }
        }

        private string GetFileName(string identificator)
        {
            return Path.Combine(m_directoryPath, identificator);
        }
    }
}