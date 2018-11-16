using System.Threading;

namespace Mediaresearch.Framework.Utilities
{
	public abstract class MultiThreadedObjectsObserverWithGroupedSortedListBase<TDicKey, TItem> : ThreadedObjectsObserverWithGroupedSortedListBase<TDicKey, TItem>
	{
	    private readonly int m_threadsCount;

		protected MultiThreadedObjectsObserverWithGroupedSortedListBase(bool runImmediately, int threadsCount) 
        {
            m_threadsCount = threadsCount;
			
            if (runImmediately)
                StartWork();
        }

        protected override void StartInternal()
        {	
            for (int i = 0; i < m_threadsCount; i++)
            {
                Thread t = new Thread(DoWork) {IsBackground = RunThreadAsBackground, Name = string.Format("{0}_{1}", WorkingThreadName, i)};
                t.Start();
            }
        }
	}
}
