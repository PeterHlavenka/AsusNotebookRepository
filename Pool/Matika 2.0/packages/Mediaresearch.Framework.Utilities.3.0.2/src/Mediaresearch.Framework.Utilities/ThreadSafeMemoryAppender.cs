using log4net.Appender;
using log4net.Core;

namespace Mediaresearch.Framework.Utilities
{
	public class ThreadSafeMemoryAppender : MemoryAppender
	{
		private object m_syncRoot = new object();
		
		public override LoggingEvent[] GetEvents()
		{
			lock(m_syncRoot)
			{
				return base.GetEvents();	
			}			
		}		

		protected override void Append(LoggingEvent loggingEvent)
		{
			lock (m_syncRoot)
			{
				base.Append(loggingEvent);
			}
		}
		
		public LoggingEvent[] GetEventsAndClear()
		{
			lock (m_syncRoot)
			{
				LoggingEvent[] events = base.GetEvents();
				base.Clear();
				return events;				
			}	
		}
	}
}
