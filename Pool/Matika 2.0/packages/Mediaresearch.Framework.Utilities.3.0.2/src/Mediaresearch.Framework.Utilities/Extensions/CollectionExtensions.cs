using System;
using System.Collections.Generic;

namespace Mediaresearch.Framework.Utilities.Extensions
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Pro kontrukce vnorenych kolekci v dictionary
		/// Prida prvek do vnoreneho listu.
		/// </summary>
		/// <typeparam name="TKey">Typ klice</typeparam>
		/// <typeparam name="TListItem">Typ prvku ve vlozene kolekci</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="key">Klic</param>
		/// <param name="item">Prvek</param>
		public static void AddItemToValueList<TKey, TListItem>(this IDictionary<TKey, List<TListItem>> dictionary, TKey key, TListItem item)
		{
			List<TListItem> list;
			dictionary.TryGetValue(key, out list);

			if (list == null)
			{
				list = new List<TListItem>();
				dictionary[key] = list;
			}

			list.Add(item);
		}

		/// <summary>
		/// Pro kontrukce vnorenych kolekci v dictionary
		/// Prida prveky do vnoreneho listu.
		/// </summary>
		/// <typeparam name="TKey">Typ klice</typeparam>
		/// <typeparam name="TListItem">Typ prvku ve vlozene kolekci</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="key">Klic</param>
		/// <param name="items">Prvky</param>
		public static void AddItemsToValueList<TKey, TListItem>(this IDictionary<TKey, List<TListItem>> dictionary, TKey key, IEnumerable<TListItem> items)
		{
			List<TListItem> list;
			dictionary.TryGetValue(key, out list);

			if (list == null)
			{
				list = new List<TListItem>();
				dictionary[key] = list;
			}

			list.AddRange(items);
		}

	    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
	    {
	        foreach (var item in list)
	        {
	            action(item);
	        }
	    }
	}
}