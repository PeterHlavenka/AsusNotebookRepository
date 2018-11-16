using System;
using System.Configuration;

namespace Mediaresearch.Framework.Utilities.Configuration.Sections
{
    public class ApplicationUpdaterConfig : ConfigBase
    {
        public override string GetConfigName()
        {
            return "ApplicationUpdaterConfiguration";
        }

        [ConfigurationProperty("UpdateCheckInterval")]
        public TimeSpan UpdateCheckInterval
        {
            get
            {
                object obj = this["UpdateCheckInterval"];
                return (TimeSpan) obj;
            }
        }

        [ConfigurationProperty("ShutdownTimeout")]
        public TimeSpan ShutdownTimeout
        {
            get
            {
                object obj = this["ShutdownTimeout"];
                return (TimeSpan) obj;
            }
        }
    }
}