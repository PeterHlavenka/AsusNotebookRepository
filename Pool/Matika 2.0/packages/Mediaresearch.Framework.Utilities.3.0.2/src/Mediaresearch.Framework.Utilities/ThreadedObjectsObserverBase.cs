using System;
using System.Collections.Generic;
using System.Threading;

namespace Mediaresearch.Framework.Utilities
{
	public interface IObjectObserver<T> : IDisposable
	{
		void HandleReceivedObjects(IEnumerable<T> objectForHandle);
	}

	public class DataHandledEventArgs<TItem> : EventArgs
	{
		private readonly TItem m_data;

		public DataHandledEventArgs(TItem data)
		{
			m_data = data;
		}

		public TItem Data
		{
			get { return m_data; }
		}
	}
	
	/// <summary>
	/// Predek pro zpracovani prichozich dat v jinem threadu. Zarizuje neblokovani threadu dodavajiciho
	/// data od jejich dalsiho zpracovani. To je jiz ponechano na implementaci potomka.
	/// </summary>
	/// <typeparam name="T">Typ zpracovavanych dat</typeparam>
	public abstract class ThreadedObjectsObserverBase<T> : IObjectObserver<T>
	{
		protected readonly ManualResetEvent m_semaphore = new ManualResetEvent(false);
		protected readonly Queue<T> m_queue = new Queue<T>();
		private bool m_disposed;
		private volatile bool m_isStoped = true;
		private volatile int m_itemsBeingHandledCount = 0;

		protected readonly object m_lockObject = new object();

		public event EventHandler<DataHandledEventArgs<T>> DataHandled;

		protected virtual void OnDataHandled(DataHandledEventArgs<T> e)
		{
			EventHandler<DataHandledEventArgs<T>> handler = DataHandled;
			if (handler != null) handler(this, e);
		}


		/// <summary>
		/// Vytvori instanci.
		/// </summary>
		/// <param name="runImmediately">Pokud true, vola se ihned metoda <see cref="StartWork"/> a 
		/// je nastartovan zpracovaci thread, jinak je nutne zavolat metodu rucne.</param>
		protected ThreadedObjectsObserverBase(bool runImmediately)
		{
			if (runImmediately)
				StartWork();
		}

		protected ThreadedObjectsObserverBase()
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

		public int WaitingItemsCount
		{
			get
			{
				lock (m_lockObject)
				{
					return m_queue.Count;
				}
			}
		}

		public int ItemsBeingHandledCount
		{
			get { return m_itemsBeingHandledCount; }
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
					m_queue.Enqueue(item);
				}
				m_semaphore.Set();
			}
		}

		/// <summary>
		/// Zastavi zpracovavani dat.
		/// </summary>
		public virtual void StopWork()
		{
			m_isStoped = true;
			m_semaphore.Set();
		}

		public virtual bool ThrowOnStartAlreadyStarted { get { return true; } }

		/// <summary>
		/// Spusti zpracovavani dat.
		/// </summary>
		public void StartWork()
		{
			if (ThrowOnStartAlreadyStarted && !m_isStoped)
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
					T objectForHandle = GetEventFromQueue();
					if (Equals(objectForHandle, default(T)))
						continue;

					try
					{
						m_itemsBeingHandledCount++;
						BeforeItemHandled(objectForHandle);
						
						HandleObject(objectForHandle);
					}
					finally
					{
						m_itemsBeingHandledCount--;
						AfterItemHandled(objectForHandle);
					}

					OnDataHandled(new DataHandledEventArgs<T>(objectForHandle));
				}
			}
		}

		protected virtual void BeforeItemHandled(T item)
		{ }

		protected virtual void AfterItemHandled(T item)
		{ }

		protected virtual T GetEventFromQueue()
		{
			T result;
			lock (m_lockObject)
			{
				if (m_queue.Count == 0)
				{					
					m_semaphore.Reset();
					return default(T);
				}

				result = m_queue.Dequeue();
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

		~ThreadedObjectsObserverBase()
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