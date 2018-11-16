using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Mediaresearch.Framework.Utilities
{
	public abstract class ThreadedObjectsObserverWithGroupedSortedListBase<TDicKey, TItem>
	{
		private class DefaultItemComparer : IComparer<TItem>
		{
			public int Compare(TItem x, TItem y)
			{
				return 1;
			}
		}

		private class DefaultKeyComparer : IComparer<TDicKey>
		{
			public int Compare(TDicKey x, TDicKey y)
			{
				return 1;
			}
		}

		protected readonly ManualResetEvent m_semaphore = new ManualResetEvent(false);
		private bool m_disposed;
		private volatile bool m_isStoped = true;
		private readonly IComparer<TItem> m_defaultItemComparer = new DefaultItemComparer();
		private readonly IComparer<TDicKey> m_defaultKeyComparer = new DefaultKeyComparer();

		protected SortedDictionary<TDicKey, List<TItem>> m_itemsCache;

		protected readonly object m_lockObject = new object();

		protected bool Disposed
        {
            get { return m_disposed; }
        }

        protected bool IsStoped
        {
            get { return m_isStoped; }
        }

        protected int WaitingItemsCount
		{
			get
			{
				lock (m_lockObject)
				{
					return m_itemsCache.Count;
				}
			}
		}

        /// <summary>
        /// Prijima data a vklada je do kolekce pro pozdejsi zpracovani. 
        /// </summary>
        /// <param name="objectForHandle">Typ zpracovavanych dat</param>
        public void HandleReceivedObjects(TDicKey key, IEnumerable<TItem> objectForHandle)
        {
            lock (m_lockObject)
            {
	            List<TItem> list;
	            m_itemsCache.TryGetValue(key, out list);
				if (list == null)
				{
					list = new List<TItem>();
					m_itemsCache[key] = list;
				}
				
				foreach (TItem item in objectForHandle)
                {
                    int pos = list.BinarySearch(item, GetItemComparer());
                    if (pos < 0)
                        pos = ~pos;

                    list.Insert(pos, item);
                }
                m_semaphore.Set();
            }
        }

        /// <summary>
        /// Vraci IComparer pro nalezeni pozice pro novy item
        /// </summary>
        /// <returns></returns>
        protected virtual IComparer<TItem> GetItemComparer()
        {
            return m_defaultItemComparer;
        }

		/// <summary>
        /// Vraci IComparer pro nalezeni pozice pro novy item
        /// </summary>
        /// <returns></returns>
        protected virtual IComparer<TDicKey> GetKeyComparer()
		{
			return m_defaultKeyComparer;
		}

        /// <summary>
        /// Zastavi zpracovavani dat.
        /// </summary>
        public void StopWork()
        {
            m_isStoped = true;
            m_semaphore.Set();
        }

        /// <summary>
        /// Spusti zpracovavani dat.
        /// </summary>
        public void StartWork()
        {
            if (!m_isStoped)
                throw new InvalidOperationException("Threaded observer already started");

            m_isStoped = false;

			m_itemsCache = new SortedDictionary<TDicKey, List<TItem>>(GetKeyComparer());

            StartInternal();
        }

        /// <summary>
        /// Nastartuje dafaultne jeden thread, virtualni pro pripaden prekryti
        /// </summary>
        protected virtual void StartInternal()
        {
            Thread workingThread = new Thread(DoWork)
                                       {
                                           Name = WorkingThreadName,
                                           IsBackground = RunThreadAsBackground
                                       };

            workingThread.Start();								
        }
		
        /// <summary>
        /// Zpracovani dat je ponechana na implementaci potomka.
        /// </summary>
        /// <param name="objectForHandle"></param>
        protected abstract void HandleObject(List<TItem> objectForHandle);
        /// <summary>
        /// Jmeno zpracovavaciho threadu.
        /// </summary>
        protected abstract string WorkingThreadName { get; }
        /// <summary>
        /// <see cref="Thread.IsBackground"/>
        /// </summary>
        protected abstract bool RunThreadAsBackground { get; }

        protected virtual void DoWork()
        {
            while (!m_isStoped)
            {
                m_semaphore.WaitOne();
                if (!m_isStoped)
                {
					List<TItem> objectForHandle = GetEventFromList();
					if (Equals(objectForHandle, default(List<TItem>)))
                        continue;

                    HandleObject(objectForHandle);
                }
            }
        }

        protected virtual List<TItem> GetEventFromList()
        {
            List<TItem> result;
            lock (m_lockObject)
            {
                if (m_itemsCache.Count == 0)
                {					
                    m_semaphore.Reset();
                    return default(List<TItem>);
                }

	            KeyValuePair<TDicKey, List<TItem>> firstKey = m_itemsCache.First();

	            result = firstKey.Value;
	            m_itemsCache.Remove(firstKey.Key);
            }

            return result;
        }

        #region IDispose Implementation

        public void Dispose()
        {
            //pass true indicating managed resources can be freed as well e.g. our code called
            //dispose instead of the .NET framework
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		~ThreadedObjectsObserverWithGroupedSortedListBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                }
                if (!m_isStoped)
                    StopWork();
            }
            m_disposed = true;
        }

        #endregion

	}
}
