using System;
using System.Collections.Generic;
using System.Configuration;

namespace Mediaresearch.Framework.Utilities.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly object m_lock = new object();
        private readonly object m_lock2 = new object();
        private readonly Dictionary<Type, string> m_sectionNames = new Dictionary<Type, string>();
        private readonly Dictionary<string, ConfigBase> m_sections = new Dictionary<string, ConfigBase>();

        public T GetConfig<T>() where T : ConfigBase
        {
            Type sectionType = typeof(T);

            AppendSectionNameIfNeeded<T>(sectionType);

            string sectionName = m_sectionNames[sectionType];

            AppendSectionIfNeeded<T>(sectionName);

            T section = (T)m_sections[sectionName];
            return section;
        }

        private void AppendSectionIfNeeded<T>(string sectionName) where T : ConfigBase
        {
            if (!m_sections.ContainsKey(sectionName))
            {
                lock (m_lock2)
                {
                    if (!m_sections.ContainsKey(sectionName))
                    {
                        T section = (T)ConfigurationManager.GetSection(sectionName);

                        if (section==null)
                        {
                            throw new ConfigurationErrorsException($"Configuration section {sectionName} does not exist.");
                        }

                        m_sections.Add(sectionName, section);
                    }
                }
            }
        }

        private void AppendSectionNameIfNeeded<T>(Type sectionType) where T : ConfigBase
        {
            if (!m_sectionNames.ContainsKey(sectionType))
            {
                lock (m_lock)
                {
                    if (!m_sectionNames.ContainsKey(sectionType))
                    {
                        var instance = Activator.CreateInstance<T>();
                        m_sectionNames.Add(sectionType, instance.GetConfigName());
                    }
                }
            }
        }
    }
}