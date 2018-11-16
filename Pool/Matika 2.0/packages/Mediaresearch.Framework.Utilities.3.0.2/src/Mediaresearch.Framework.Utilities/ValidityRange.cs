using System;
using Mediaresearch.Framework.Utilities.DataStructures;

namespace Mediaresearch.Framework.Domain.History
{
	/// <summary>
	/// Specialni uprava <see cref="DateTimeRange"/> jez je vzdy otevrena zprava a uklada cas s presnosti na jednu vterinu.
	/// </summary>
	[Serializable]
	public sealed class ValidityRange: DateTimeRange
	{
		private ValidityRange()
			: this(DateTime.MinValue, DateTime.MinValue, RangeStates.OpenFromRight, TimeSpan.FromSeconds(1))
		{			
		}

		public ValidityRange(DateTime start, DateTime end)
			: this(RoundDateTime(start), RoundDateTime(end), RangeStates.OpenFromRight, TimeSpan.FromSeconds(1))
		{			
		}

		private ValidityRange(DateTime start, DateTime end, RangeStates rangeStatus, TimeSpan step)
			: base(RoundDateTime(start), RoundDateTime(end), rangeStatus, step)
		{
			// just hides constructor
		}

		public static DateTime RoundDateTime(DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", From, To);
		}

		public override bool Contains(DateTime element)
		{
			return base.Contains(RoundDateTime(element));
		}
	}
}