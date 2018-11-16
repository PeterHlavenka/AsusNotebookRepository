using System;
using System.Collections.Generic;

namespace Mediaresearch.Framework.Utilities
{
	public class SortedItemWrapper<T> : ItemWrapper<T>, IComparable<SortedItemWrapper<T>>
	{
		public IComparer<T> ItemComparer { get; internal set; }

		public int CompareTo(SortedItemWrapper<T> other)
		{
			int itemCompareResult = ItemComparer.Compare(Item, other.Item);

			if ((itemCompareResult < 0 && DequeueTime <= DateTime.Now) || 
				(itemCompareResult > 0 && other.DequeueTime <= DateTime.Now))
				return -itemCompareResult;

			return base.CompareTo(other);
		}
	}

	public class SortedTimeQueue<T> : TimeQueueBase<T, SortedItemWrapper<T>>
	{
		private readonly IComparer<T> m_itemComparer;

		public SortedTimeQueue(IComparer<T> itemComparer)
			:this(itemComparer, 64)
		{
		}

		public SortedTimeQueue(IComparer<T> itemComparer, int initialCapacity) 
			: base(initialCapacity)
		{
			m_itemComparer = itemComparer;
		}

		protected override int GetNewItemIndex(SortedItemWrapper<T> newItem)
		{
			newItem.ItemComparer = m_itemComparer;
			return base.GetNewItemIndex(newItem);
		}
	}
}
