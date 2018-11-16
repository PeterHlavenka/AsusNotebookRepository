using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Mediaresearch.Framework.DataBinding
{
	public class SortableSearchableList<T> : BindingList<T>
	{
		public SortableSearchableList(IList<T> list):base(list)
		{
		}

		private ListSortDirection m_sortDirectionValue;
		private PropertyDescriptor m_sortPropertyValue;
		private bool m_isSortedValue;
		
		protected override PropertyDescriptor SortPropertyCore
		{
			get { return m_sortPropertyValue; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get { return m_sortDirectionValue; }
		}

		protected override bool IsSortedCore
		{
			get { return m_isSortedValue; }
		}

		public override void EndNew(int itemIndex)
		{
			if (m_sortPropertyValue != null && itemIndex == Count - 1)
				ApplySortCore(m_sortPropertyValue, m_sortDirectionValue);
			base.EndNew(itemIndex);
		}
		
		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		public void RemoveSort()
		{
			RemoveSortCore();
		}

		private ArrayList m_sortedList;
		private ArrayList m_unsortedItems;

		public SortableSearchableList()
		{
			
		}

		protected override void ApplySortCore(PropertyDescriptor prop,
		                                      ListSortDirection direction)
		{
			m_sortedList = new ArrayList();

			// Check to see if the property type we are sorting by implements
			// the IComparable interface.
			Type interfaceType = prop.PropertyType.GetInterface("IComparable");

			if (interfaceType != null)
			{
				// If so, set the SortPropertyValue and SortDirectionValue.
				m_sortPropertyValue = prop;
				m_sortDirectionValue = direction;

				m_unsortedItems = new ArrayList(Count);

				// Loop through each item, adding it the the sortedItems ArrayList.
				foreach (Object item in Items)
				{
					m_sortedList.Add(prop.GetValue(item));
					m_unsortedItems.Add(item);
				}

				// Call Sort on the ArrayList.
				m_sortedList.Sort();
				T temp;

				// Check the sort direction and then copy the sorted items
				// back into the list.
				if (direction == ListSortDirection.Descending)
					m_sortedList.Reverse();

				for (int i = 0; i < Count; i++)
				{
					int position = Find(prop.Name, m_sortedList[i]);
					if (position != i)
					{
						temp = this[i];
						this[i] = this[position];
						this[position] = temp;
					}
				}

				m_isSortedValue = true;

				// Raise the ListChanged event so bound controls refresh their
				// values.
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
			else
				// If the property type does not implement IComparable, let the user
				// know.
				throw new NotSupportedException("Cannot sort by " + prop.Name + ". This" +
				                                prop.PropertyType.ToString() + " does not implement IComparable");
		}

		protected override void RemoveSortCore()
		{
			int position;
			object temp;

			// Ensure the list has been sorted.
			if (m_unsortedItems != null)
			{
				// Loop through the unsorted items and reorder the
				// list per the unsorted list.
				for (int i = 0; i < m_unsortedItems.Count;)
				{
					position = Find("LastName",
					                m_unsortedItems[i].GetType().
					                	GetProperty("LastName").GetValue(m_unsortedItems[i], null));
					if (position >= 0 && position != i)
					{
						temp = this[i];
						this[i] = this[position];
						this[position] = (T) temp;
						i++;
					}
					else if (position == i)
						i++;
					else
						// If an item in the unsorted list no longer exists, delete it.
						m_unsortedItems.RemoveAt(i);
				}

				m_isSortedValue = false;

				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
		}

		protected override bool SupportsSearchingCore
		{
			get { return true; }
		}

		protected override int FindCore(PropertyDescriptor prop, object key)
		{
			// Get the property info for the specified property.
			PropertyInfo propInfo = typeof (T).GetProperty(prop.Name);
			T item;

			if (key != null)
			{
				// Loop through the the items to see if the key
				// value matches the property value.
				for (int i = 0; i < Count; ++i)
				{
					item = (T) Items[i];
					if (propInfo.GetValue(item, null).Equals(key))
						return i;
				}
			}
			return -1;
		}

		public int Find(string property, object key)
		{
			// Check the properties for a property with the specified name.
			PropertyDescriptorCollection properties =
				TypeDescriptor.GetProperties(typeof (T));
			PropertyDescriptor prop = properties.Find(property, true);

			// If there is not a match, return -1 otherwise pass search to
			// FindCore method.
			if (prop == null)
				return -1;
			else
				return FindCore(prop, key);
		}

		/// <summary>
		/// Vytvori z generickeho SortableSearchableList genericky List. 
		/// </summary>
		/// <returns></returns>
		public List<T> ToList()
		{			
			List<T> result = new List<T>();
			foreach(T item in this)
			{
				result.Add(item);
			}
			return result;
		}
	}
}