using System;
using Mediaresearch.Framework.Utilities.DataStructures;

namespace Mediaresearch.Framework.Utilities
{
	public interface IRange<TInterval, TStep>
		where TInterval : IComparable, IComparable<TInterval>
		where TStep : IComparable, IComparable<TStep>
	{
		RangeStates RangeStatus { get; }
		TStep Step { get; }
		TInterval Start { get; }
		TInterval End { get; }
		bool IsOverlap(RangeBase<TInterval, TStep> other);
		bool Contains(RangeBase<TInterval, TStep> other);
		bool Contains(TInterval element);
		bool Abuts(RangeBase<TInterval, TStep> other);
		/// <summary>
		/// Vrati interval mezi intervaly. V pripade, ze existuje prunik, vrati prazdny interval.
		/// </summary>
		RangeBase<TInterval, TStep> Gap(RangeBase<TInterval, TStep> other);
		bool IsEmpty { get; }
		RangeBase<TInterval, TStep> GetOverlap(RangeBase<TInterval, TStep> other);
		RangeBase<TInterval, TStep> GetInstance(TInterval start, TInterval end, RangeStates rangeStatus, TStep step);
	}
}