using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;

namespace Mediaresearch.Framework.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
    public class LocalizationResourceAttribute : Attribute
    {
        public LocalizationResourceAttribute(Type resourceType)
        {
            if (resourceType == null) throw new ArgumentNullException(nameof(resourceType));
            ResourceType = resourceType;
        }

        public Type ResourceType { get; }
    }

    public interface ILocalizationProvider
    {
        string Localize<T>(T value);
    }

    public class LocalizationProvider : ILocalizationProvider
    {
        private readonly Dictionary<Type, Dictionary<object, string>> m_cache = new Dictionary<Type, Dictionary<object, string>>();
        private static readonly Dictionary<Type, ResourceManager> m_resourceManagers = new Dictionary<Type, ResourceManager>();

        public string Localize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            lock (((ICollection)m_cache).SyncRoot)
            {
                Type type = typeof(T);
                
                string valueString = value.ToString();

                if (!m_cache.ContainsKey(type))
                {
                    m_cache.Add(type, new Dictionary<object, string>());
                }

                Dictionary<object, string> cache = m_cache[type];

                string localization;
                if (cache.TryGetValue(value, out localization))
                {
                    return localization;
                }

                var attribute = type.GetCustomAttributes(typeof(LocalizationResourceAttribute), false).FirstOrDefault() as LocalizationResourceAttribute;
                if (attribute?.ResourceType == null)
                {
                    return valueString;
                }

                if (!m_resourceManagers.ContainsKey(attribute.ResourceType))
                {
                    m_resourceManagers.Add(attribute.ResourceType, new ResourceManager(attribute.ResourceType));
                }

                localization = m_resourceManagers[attribute.ResourceType].GetString($"{type.Name}_{valueString}", CultureInfo.CurrentUICulture);

                cache.Add(value, localization);

                m_cache[type] = cache;

                return localization;

            }
        }
    }
}
