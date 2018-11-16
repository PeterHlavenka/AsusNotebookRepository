using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using log4net;

namespace Mediaresearch.Framework.Utilities
{
		public class ItemWrapper<T> : IComparable<ItemWrapper<T>>
		{
			public T Item { get; internal set; }

			public DateTime DequeueTime { get; private set; }

			public void SetTimeout(TimeSpan timeout)
			{
				DequeueTime = DateTime.Now + timeout;
			}

			public void SetTimeout(int seconds)
			{
				SetTimeout(TimeSpan.FromSeconds(seconds));
			}

			public void SetTimeout(DateTime dateTime)
			{
				DequeueTime = dateTime;
			}

			#region IComparable<ItemWrapper> Members

			public int CompareTo(ItemWrapper<T> wrapper)
			{
				return -1 * (DequeueTime.CompareTo(wrapper.DequeueTime));
			}

			#endregion
		}
	
	public class TimeQueue<T> : TimeQueueBase<T, ItemWrapper<T>>
	{
		public TimeQueue()
		{
		}

		public TimeQueue(int initialCapacity) : base(initialCapacity)
		{
		}
	}

	public class TimeQueueBase<T, W>
		where W : ItemWrapper<T>, new()
	{
		private static readonly ILog m_log = LogManager.GetLogger("Mediaresearch.Framework.Utilities.TimeQueueBase");
		private const string m_logDateFormat = "dd.MM.yy HH:mm:ss,ffff";

		private readonly EventWaitHandle m_queueEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
		private const int m_defaultDeQueueTimeout = 100;

		private List<W> m_list;

		protected List<W> List
		{
			get { return m_list; }
		}

		#region Constructors
		
		public TimeQueueBase()
			:this(64)
		{
		}

		public TimeQueueBase(int initialCapacity)
		{
			m_list = new List<W>(initialCapacity);
		}

		#endregion					
		
		public int ItemsCount
		{
			[DebuggerStepThrough()]
			get
			{
				lock (SyncRoot)
				{
					return m_list.Count;
				}
			}
		}

		public void EnQueue(T item, TimeSpan timeout)
		{
			W newItem = new W {Item = item};
			newItem.SetTimeout(timeout);

			EnQueueProcess(newItem);
		}

		public void EnQueue(T item, DateTime dateTime)
		{
			W newItem = new W { Item = item };
			newItem.SetTimeout(dateTime);

			EnQueueProcess(newItem);
		}

		public void Enqueue(List<T> items, DateTime dateTime)
		{
			List<W> wrappedItems = new List<W>(items.Count);
			foreach (T item in items)
			{
				W wrappedItem = new W {Item = item};
				wrappedItem.SetTimeout(dateTime);
				wrappedItems.Add(wrappedItem);
			}

			EnQueueAllProcesses(wrappedItems);
		}

		protected virtual int GetNewItemIndex(W newItem)
		{
			return m_list.BinarySearch(newItem);			
		}

		protected void EnQueueProcess(W newItem)
		{
			if (m_log.IsInfoEnabled)
				m_log.InfoFormat("Enqueuing new item (Dequeue time: '{0}' ; Now: '{1}' ; Milliseconds: {2} )",
					newItem.DequeueTime.ToString(m_logDateFormat), DateTime.Now.ToString(m_logDateFormat), newItem.DequeueTime.Subtract(DateTime.Now).TotalMilliseconds);

			lock (SyncRoot)
			{
				EnqueueProcessInternal(newItem);
			}
			m_queueEvent.Set();
		}

		protected void EnQueueAllProcesses(List<W> newItems)
		{
			if (m_log.IsDebugEnabled)
				m_log.DebugFormat("Enqueueing {0} items...", newItems.Count);

			int insertedItemsCounter = 0;
			lock (SyncRoot)
			{
// ReSharper disable LoopCanBeConvertedToQuery
				foreach (W newItem in newItems)
// ReSharper restore LoopCanBeConvertedToQuery
				{
					if (EnqueueProcessInternal(newItem))
						insertedItemsCounter++;
				}
			}

			if (m_log.IsDebugEnabled)
				m_log.DebugFormat("From {0} items {1} were really inserted", newItems.Count, insertedItemsCounter);

			m_queueEvent.Set();
		}

		protected virtual bool EnqueueProcessInternal(W newItem)
		{
			int index = GetNewItemIndex(newItem);

			if (m_log.IsDebugEnabled)
				m_log.DebugFormat("Inserting new item on index {0}", index < 0 ? ~index : index);

			if (index < 0)
				InsertItem(~index, newItem);
			else
				InsertItem(index, newItem);

			return true;
		}

		protected virtual void InsertItem(int index, W newItem)
		{
			m_list.Insert(index, newItem);
		}

		public T DeQueue()
		{
			return DeQueue(m_defaultDeQueueTimeout);
		}

		public T DeQueue(int dequeueTimeout)
		{
			if (dequeueTimeout < 0) throw new ArgumentOutOfRangeException("dequeueTimeout", "Dequeue timeout should be >= 0");

			if(m_log.IsDebugEnabled)
				m_log.DebugFormat("DeQueue called with max dequeue timeout value {0} ms", dequeueTimeout);

			int timeout = 0;
			bool doDequeueOnTimeout = false;
			//int itemTimeout = 0;
			W firstItem;
			Stopwatch sw = new Stopwatch();

			while (true)
			{
				timeout = dequeueTimeout;

				// zjisti pozadovany timeout
				lock (SyncRoot)
				{
					if (m_list.Count > 0)
					{
						firstItem = m_list[m_list.Count - 1];
						int itemTimeout = (int)firstItem.DequeueTime.Subtract(DateTime.Now).TotalMilliseconds;
						
						if (m_log.IsDebugEnabled)
							m_log.DebugFormat("Queue contains {0} items, first item timeout = {1} ms", m_list.Count, itemTimeout);

						// pokud ma prvni item zaporny timeout, radeji ji hned vratime (zabranime tim zablokovani dequeu z duvodu neustaleho vkladani novych polozek)
						if (itemTimeout < 0)
						{														
							RemoveItem(m_list.Count - 1, firstItem);
							break;
						}
						
						if (dequeueTimeout < itemTimeout)
						{
							timeout = dequeueTimeout;
							doDequeueOnTimeout = false;
						}
						else
						{
							timeout = itemTimeout;
							doDequeueOnTimeout = true;
						}
					}
				}

				sw.Reset();				
				if (m_log.IsDebugEnabled)
					m_log.DebugFormat("Stopwatch started, going to wait for {0} ms...", timeout);
				sw.Start();

				//HACK
				if (timeout < 0)
					timeout = 1;
					
				
				bool isSignaled = m_queueEvent.WaitOne(timeout, false);
				sw.Stop();
				dequeueTimeout -= (int)sw.ElapsedMilliseconds;

				if (!isSignaled)
				{	
					// dobehnul timeout
					if (m_log.IsDebugEnabled)
						m_log.DebugFormat("Thread waked up after {0} ms due to timeout", sw.ElapsedMilliseconds);
					
					if(!doDequeueOnTimeout)
					{
						if (m_log.IsDebugEnabled)
							m_log.Debug("Dequeue timeout elapsed, returning null");
						
						return default(T);
					}
					else
					{
						if (m_log.IsDebugEnabled)
							m_log.Debug("Item timeout elapsed");
						
						lock (SyncRoot)
						{
							//menim implementaci z chyby na warning, protoze:
							//pokud je aplikace vicevlaknova, muze se dostat k radku s m_queueEvent.WaitOne vice vlaken s doDequeueOnTimeout na true
							//ovsem pokud byla v m_list pouze jedna polozka, vyzere ji prvni vlakno, kdezto druhe pote vytimeoutuje a dostane se k tomuto kodu,
							//i kdyz je uz m_list prazdny
							//Debug.Assert(m_list.Count > 0, "Item timeout expired but there are no items in the queue!");

							if (!m_list.Any())
							{
								if (m_log.IsDebugEnabled)
									m_log.Debug("Item timeout expired but there are no items in the queue - returning empty item");
								return default(T);
							}

							firstItem = m_list[m_list.Count - 1];
							RemoveItem(m_list.Count - 1, firstItem);
						}
						break;
					}
				}

				// event byl signaled z metody EnQueue
				if (m_log.IsDebugEnabled)
					m_log.DebugFormat("Thread waked up by signal - going to check new item and wait for the rest of max dequeu timeout ({0} ms)", dequeueTimeout);
			}
			
			if (m_log.IsInfoEnabled)
				m_log.InfoFormat("Dequeuing item (planned dequeue time: {0} ; Now: {1}; Items left: {2})",
					firstItem.DequeueTime.ToString(m_logDateFormat), DateTime.Now.ToString(m_logDateFormat), m_list.Count);
			return firstItem.Item;
		}

		protected virtual void RemoveItem(int index, W item)
		{
			m_list.RemoveAt(index);
		}

		public T Peek()
		{
			lock (SyncRoot)
			{
				return m_list.Count > 0 ? m_list[m_list.Count - 1].Item : default(T);
			}
		}

		public object SyncRoot
		{
			get { return ((ICollection)List).SyncRoot; }
		}
	}
}