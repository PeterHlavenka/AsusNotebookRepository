using System;
using System.Collections.Generic;
using System.Threading;

namespace Mediaresearch.Framework.Utilities
{
    /// <summary>
    /// Predek pro zpracovani prichozich dat v jinem threadu. Zarizuje neblokovani threadu dodavajiciho
    /// data od jejich dalsiho zpracovani. To je jiz ponechano na implementaci potomka.
    /// Pridavane polozky se vkladaji do vnitrniho listu na pozici dle implementace ItemComparer.
    /// </summary>
    /// <typeparam name="T">Typ zpracovavanych dat</typeparam>
    public abstract class ThreadedObjectsObserverWithSortedListBase<T> : IObjectObserver<T>
    {
        private class DefaultComparer : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return 1;
            }
        }

        protected readonly ManualResetEvent m_semaphore = new ManualResetEvent(false);
        protected readonly List<T> m_list = new List<T>();
        private bool m_disposed;
        private volatile bool m_isStoped = true;
        private IComparer<T> m_defaultComparer = new DefaultComparer();

        protected readonly object m_lockObject = new object();

        /// <summary>
        /// Vytvori instanci.
        /// </summary>
        /// <param name="runImmediately">Pokud true, vola se ihned metoda <see cref="StartWork"/> a 
        /// je nastartovan zpracovaci thread, jinak je nutne zavolat metodu rucne.</param>
        protected ThreadedObjectsObserverWithSortedListBase(bool runImmediately)
        {
            if (runImmediately)
                StartWork();
        }

        protected ThreadedObjectsObserverWithSortedListBase()
        {
        }

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
					return m_list.Count;
				}
			}
		}

        /// <summary>
        /// Prijima data a vklada je do kolekce pro pozdejsi zpracovani. 
        /// </summary>
        /// <param name="objectForHandle">Typ zpracovavanych dat</param>
        public void HandleReceivedObjects(IEnumerable<T> objectForHandle)
        {
            lock (m_lockObject)
            {
                foreach (T item in objectForHandle)
                {
                    int pos = m_list.BinarySearch(item, GetItemComparer());
                    if (pos < 0)
                        pos = ~pos;

                    m_list.Insert(pos, item);
                }
                m_semaphore.Set();
            }
        }

        /// <summary>
        /// Vraci IComparer pro nalezeni pozice pro novy item
        /// </summary>
        /// <returns></returns>
        protected virtual IComparer<T> GetItemComparer()
        {
            return m_defaultComparer;
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
            StartInternal();
        }

        /// <summary>
        /// Nastartuje dafaultne jedenm thread, virtualni pro pripaden prekryti
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
        protected abstract void HandleObject(T objectForHandle);
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
                    T objectForHandle = GetEventFromList();
                    if (Equals(objectForHandle, default(T)))
                        continue;

                    HandleObject(objectForHandle);
                }
            }
        }

        protected virtual T GetEventFromList()
        {
            T result;
            lock (m_lockObject)
            {
                if (m_list.Count == 0)
                {					
                    m_semaphore.Reset();
                    return default(T);
                }

                result = m_list[0];
                m_list.RemoveAt(0);
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

        ~ThreadedObjectsObserverWithSortedListBase()
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