using System.Threading;

namespace Mediaresearch.Framework.Utilities
{
    /// <summary>
    /// Vice threadovy observer s razenim zpracovavanych polozek (napr. dle priority). V konstuktoru predane mnozstvi threadu bude zpracovavat objekty ve fronte.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MultiThreadedObjectsObserverWithSortedListBase<T> : ThreadedObjectsObserverWithSortedListBase<T>
    {
        private readonly int m_threadsCount;

        protected MultiThreadedObjectsObserverWithSortedListBase(bool runImmediately, int threadsCount) 
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