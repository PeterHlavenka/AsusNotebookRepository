using System.Collections.Generic;

namespace Mediaresearch.Framework.Utilities
{
	public class UniqueItemsSortedTimeQueue<T> : SortedTimeQueue<T>
	{
		private readonly HashSet<T> m_set = new HashSet<T>();
  
		public UniqueItemsSortedTimeQueue(IComparer<T> itemComparer) : base(itemComparer)
		{
		}

		public UniqueItemsSortedTimeQueue(IComparer<T> itemComparer, int initialCapacity) : base(itemComparer, initialCapacity)
		{
		}

		protected override bool EnqueueProcessInternal(SortedItemWrapper<T> newItem)
		{
			if (m_set.Contains(newItem.Item))
				return false;

			return base.EnqueueProcessInternal(newItem);
		}

		protected override void InsertItem(int index, SortedItemWrapper<T> newItem)
		{
			base.InsertItem(index, newItem);

			m_set.Add(newItem.Item);
		}

		protected override void RemoveItem(int index, SortedItemWrapper<T> item)
		{
			base.RemoveItem(index, item);

			m_set.Remove(item.Item);
		}
	}
}