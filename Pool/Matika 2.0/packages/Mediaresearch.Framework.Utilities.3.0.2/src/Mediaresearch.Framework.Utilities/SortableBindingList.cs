using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Mediaresearch.Framework.Utilities
{
	public class SortableBindingList<T> : BindingList<T>
	{
		private List<T> m_originalList;

		private ListSortDirection m_sortDirection;

		private PropertyDescriptor m_sortProperty;

		private readonly Action<SortableBindingList<T>, List<T>> m_populateBaseList = (a, b) => a.ResetItems(b);

		private static readonly Dictionary<string, Func<List<T>, IEnumerable<T>>> m_cachedOrderByExpressions = new Dictionary<string, Func<List<T>, IEnumerable<T>>>();

		public SortableBindingList()
		{
			m_originalList = new List<T>();
		}

		public SortableBindingList(IEnumerable<T> enumerable)
		{
			m_originalList = enumerable.ToList();
			m_populateBaseList(this, m_originalList);
		}

		public SortableBindingList(List<T> list)
		{
			m_originalList = list;
			m_populateBaseList(this, m_originalList);
		}

		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			m_sortProperty = prop;

			var orderByMethodName = m_sortDirection == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";

			var cacheKey = typeof(T).GUID + prop.Name + orderByMethodName;

			if (!m_cachedOrderByExpressions.ContainsKey(cacheKey))
			{
				CreateOrderByMethod(prop, orderByMethodName, cacheKey);
			}

			ResetItems(m_cachedOrderByExpressions[cacheKey](m_originalList).ToList());

			ResetBindings();

			m_sortDirection = m_sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
		}

		private static void CreateOrderByMethod(PropertyDescriptor prop, string orderByMethodName, string cacheKey)
		{
			var sourceParameter = Expression.Parameter(typeof(List<T>), "source");

			var lambdaParameter = Expression.Parameter(typeof(T), "lambdaParameter");

			var accesedMember = typeof(T).GetProperty(prop.Name);

			var propertySelectorLambda =
				Expression.Lambda(Expression.MakeMemberAccess(lambdaParameter, accesedMember), lambdaParameter);

			var orderByMethod = typeof(Enumerable).GetMethods()
												  .Where(a => a.Name == orderByMethodName && a.GetParameters().Length == 2)
												  .Single()
												  .MakeGenericMethod(typeof(T), prop.PropertyType);

			var orderByExpression = Expression.Lambda<Func<List<T>, IEnumerable<T>>>(
										Expression.Call(orderByMethod,
														new Expression[] { sourceParameter, propertySelectorLambda }), sourceParameter);

			m_cachedOrderByExpressions.Add(cacheKey, orderByExpression.Compile());
		}

		protected override void RemoveSortCore()
		{

			ResetItems(m_originalList);
		}

		private void ResetItems(List<T> items)
		{

			base.ClearItems();

			for (int i = 0; i < items.Count; i++)
			{
				base.InsertItem(i, items[i]);
			}

		}

		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get { return m_sortDirection; }
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get { return m_sortProperty; }
		}

		protected override void OnListChanged(ListChangedEventArgs e)
		{
			m_originalList = Items.ToList();
		}
	}
}
