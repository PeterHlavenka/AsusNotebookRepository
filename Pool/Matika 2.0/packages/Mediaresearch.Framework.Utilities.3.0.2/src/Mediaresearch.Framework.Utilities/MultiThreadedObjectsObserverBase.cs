using System.Threading;

namespace Mediaresearch.Framework.Utilities
{
	/// <summary>
	/// Vice threadovy observer. V konstuktoru predane mnozstvi threadu bude zpracovavat objekty ve fronte.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class MultiThreadedObjectsObserverBase<T> : ThreadedObjectsObserverBase<T>
	{
		private readonly int m_threadsCount;

		protected MultiThreadedObjectsObserverBase(bool runImmediately, int threadsCount) 
		{
			m_threadsCount = threadsCount;
			
			if (runImmediately)
				StartWork();
		}

		public int ThreadsCount
		{
			get { return m_threadsCount; }
		}

		protected override void StartInternal()
		{	
			for (int i = 0; i < ThreadsCount; i++)
			{
				Thread t = new Thread(DoWork) {IsBackground = RunThreadAsBackground, Name = string.Format("{0}_{1}", WorkingThreadName, i)};
				t.Start();
			}
		}
	}
}
