using System;
using Mediaresearch.Framework.Utilities.DataStructures;

namespace Mediaresearch.Framework.Utilities
{
	public abstract class RangeBase<TInterval, TStep> : IRange<TInterval, TStep> where TInterval : IComparable, IComparable<TInterval> where TStep : IComparable, IComparable<TStep>
	{
		private readonly TInterval m_end;
		private readonly RangeStates m_rangeStatus = RangeStates.Closed;
		private readonly TInterval m_start;
		private readonly TStep m_step;

		protected RangeBase()
		{
		}

		protected RangeBase(TInterval start, TInterval end, TStep step)
			: this(start, end, RangeStates.Closed, step)
		{
		}

		protected RangeBase(TInterval start, TInterval end, RangeStates rangeStatus, TStep step)
		{
			if (start.CompareTo(end) > 0)
				throw new ArgumentException("Start interval must by greater than end.");
			m_rangeStatus = rangeStatus;
			m_start = start;
			m_end = end;
			m_step = step;
		}

		public TInterval RealStart
		{
			get { return RangeStatus == RangeStates.OpenFromLeft || RangeStatus == RangeStates.OpenFromBoth ? AddStep(Start, Step) : Start; }
		}

		public TInterval RealEnd
		{
			get { return RangeStatus == RangeStates.OpenFromRight || RangeStatus == RangeStates.OpenFromBoth ? SubstractStep(End, Step) : End; }
		}

		public abstract RangeBase<TInterval, TStep> Empty { get; }

		protected virtual TInterval Zero
		{
			get { return Activator.CreateInstance<TInterval>(); }
		}

		#region IRange<TInterval,TStep> Members

		public RangeStates RangeStatus
		{
			get { return m_rangeStatus; }
		}

		public TStep Step
		{
			get { return m_step; }
		}

		public TInterval Start
		{
			get { return m_start; }
		}

		public TInterval End
		{
			get { return m_end; }
		}

		public bool IsOverlap(RangeBase<TInterval, TStep> other)
		{
			if (other.IsEmpty)
				return false;

			if (RealStart.CompareTo(other.RealEnd) <= 0 && RealEnd.CompareTo(other.RealStart) >= 0)
				return true;

			return false;
		}

		public bool Contains(RangeBase<TInterval, TStep> other)
		{
			if (other.IsEmpty)
				return false;
			if (!IsOverlap(other))
				return false;
			//if ((other.RealStart >= this.RealStart) && (other.RealEnd <= this.RealEnd))
			if (other.RealStart.CompareTo(RealStart) >= 0 && other.RealEnd.CompareTo(RealEnd) <= 0)
				return true;
			return false;
		}

		public TInterval Lenght
		{
			get
			{
				TInterval interval = Gap(RealStart, RealEnd);
				return interval.CompareTo(Zero) < 0 ? Zero : interval;
			}
		}

		public bool Contains(TInterval element)
		{
			//return (element >= this.RealStart && element <= this.RealEnd);
			return (element.CompareTo(RealStart) >= 0 && element.CompareTo(RealEnd) <= 0);
		}

		public bool Abuts(RangeBase<TInterval, TStep> other)
		{
			RangeBase<TInterval, TStep> gap = Gap(other);
			return gap.IsEmpty;
		}

		
		public RangeBase<TInterval, TStep> Gap(RangeBase<TInterval, TStep> other)
		{
			if(Step.CompareTo(other.Step) != 0)
				throw new InvalidOperationException("Unable to get Gap of ranges with different steps!");

			if (other.IsEmpty)
				return Empty;
			if (IsOverlap(other))
				return Empty;
			RangeStates overlapStatus = RangeStates.None;
			if (other.RealStart.CompareTo(RealStart) > 0)
			{
				if ((other.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if ((RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return GetInstance(End, other.Start, overlapStatus, Step);
			}

			if ((RangeStatus & RangeStates.OpenFromLeft) > 0)
				overlapStatus |= RangeStates.OpenFromRight;
			if ((other.RangeStatus & RangeStates.OpenFromRight) > 0)
				overlapStatus |= RangeStates.OpenFromLeft;
			if (overlapStatus == RangeStates.None)
				overlapStatus = RangeStates.Closed;
			return GetInstance(other.End, Start, overlapStatus, Step);
		}

		public bool IsEmpty
		{
			get
			{
				switch (RangeStatus)
				{
					case RangeStates.None:
					case RangeStates.Closed:
						return Gap(Start, End).CompareTo(Zero) < 0;
					case RangeStates.OpenFromBoth:
						return Gap(RealStart, RealEnd).CompareTo(Zero) <= 0;
					case RangeStates.OpenFromLeft:
						return Gap(RealStart, End).CompareTo(SubstractStep(Zero, Step)) < 0;
					case RangeStates.OpenFromRight:
						return Gap(Start, RealEnd).CompareTo(SubstractStep(Zero, Step)) < 0;
					default:
						throw new NotImplementedException("Have you forgotten to implement new RangeStatus?");
				}
				//return Gap(RealStart, RealEnd).CompareTo(Zero) <= 0;
			}
		}

		public RangeBase<TInterval, TStep> GetOverlap(RangeBase<TInterval, TStep> other)
		{
			if (other.IsEmpty)
				return Empty;
			if (!IsOverlap(other))
				return Empty;
			if (!other.IsOverlap(this))
				return Empty;
			if (Contains(other))
				return other;
			if (other.Contains(this))
				return this;
			RangeStates overlapStatus = RangeStates.None;
			if (other.RealStart.CompareTo(RealStart) >= 0)
			{
				if ((other.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if ((RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return GetInstance(other.Start, End, overlapStatus, Step);
			}

			if ((RangeStatus & RangeStates.OpenFromLeft) > 0)
				overlapStatus |= RangeStates.OpenFromLeft;
			if ((other.RangeStatus & RangeStates.OpenFromRight) > 0)
				overlapStatus |= RangeStates.OpenFromRight;
			if (overlapStatus == RangeStates.None)
				overlapStatus = RangeStates.Closed;
			return GetInstance(Start, other.End, overlapStatus, Step);
		}

		#endregion

		protected abstract TInterval AddStep(TInterval intevalBoundary, TStep step);
		protected abstract TInterval SubstractStep(TInterval intervalBoundary, TStep step);
		protected abstract TInterval Gap(TInterval start, TInterval end);
		protected abstract TStep GapInStep(TInterval start, TInterval end);
		public abstract RangeBase<TInterval, TStep> GetInstance(TInterval start, TInterval end, RangeStates rangeStatus, TStep step);

		public override bool Equals(object obj)
		{
			var compareRange = obj as RangeBase<TInterval, TStep>;
			if (compareRange == null)
				return false;
			if (m_rangeStatus != compareRange.RangeStatus)
				return false;
			if (m_start.Equals(compareRange.Start) && m_end.Equals(compareRange.End))
				return true;
			return false;
		}

		public int CompareTo(RangeBase<TInterval, TStep> other)
		{
			if (!RealStart.Equals(other.RealStart)) return RealStart.CompareTo(other.RealStart);
			return RealEnd.CompareTo(other.RealEnd);
		}

		public override int GetHashCode()
		{
			return ((Start.ToString().GetHashCode()) + (End.ToString().GetHashCode()));
		}

		//public static bool operator <(RangeBase<TInterval, TStep> left, RangeBase<TInterval, TStep> right)
		//{
		//    return left.RealStart.CompareTo(right.RealStart) < 0;
		//}

		//public static bool operator >(RangeBase<TInterval, TStep> left, RangeBase<TInterval, TStep> right)
		//{
		//    return left.RealStart.CompareTo(right.RealStart) > 0;
		//}

		//public static bool operator <=(RangeBase<TInterval, TStep> left, RangeBase<TInterval, TStep> right)
		//{
		//    return left.RealStart.CompareTo(right.RealStart) <= 0;
		//}

		//public static bool operator >=(RangeBase<TInterval, TStep> left, RangeBase<TInterval, TStep> right)
		//{
		//    return left.RealStart.CompareTo(right.RealStart) >= 0;
		//}

		//public static RangeBase<TInterval, TStep> operator -(RangeBase<TInterval, TStep> left, RangeBase<TInterval, TStep> right)
		//{
		//    if(left.Step.CompareTo(right.Step) != 0)
		//        throw new InvalidOperationException("Can't substract interval with different steps!");
		//    if(left.IsOverlap(right))
		//        return left < right 
		//            ? left.GetInstance(left.Start, right.Start, CombineRangesForSubstract(left.RangeStatus, right.RangeStatus), left.Step)
		//            : right.GetInstance(right.Start, left.Start, CombineRangesForSubstract(right.RangeStatus, left.RangeStatus), left.Step);
		//    return left;
		//}


		//protected static RangeStates CombineRangesForSubstract(RangeStates leftRangeStatus, RangeStates rightRangeStatus)
		//{
		//    bool openLeft = leftRangeStatus == RangeStates.OpenFromBoth || leftRangeStatus == RangeStates.OpenFromLeft;
		//    bool openRight = rightRangeStatus == RangeStates.OpenFromBoth || rightRangeStatus == RangeStates.OpenFromRight;
		//    return openLeft && openRight ? RangeStates.OpenFromBoth :
		//                                                                openLeft ? RangeStates.OpenFromLeft :
		//                                                                                                        openRight ? RangeStates.OpenFromRight :
		//                                                                                                                                                RangeStates.Closed;
		//}
	}
}