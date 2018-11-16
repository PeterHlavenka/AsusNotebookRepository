using System;
using System.Runtime.Serialization;

namespace Mediaresearch.Framework.Utilities.DataStructures
{
	[Flags]
	[DataContract]
	public enum RangeStates
	{
		[EnumMember]
		None = 0x00,
		[EnumMember]
		Closed = 0x01,
		[EnumMember]
		OpenFromLeft = 0x02,
		[EnumMember]
		OpenFromRight = 0x04,
		[EnumMember]
		OpenFromBoth = OpenFromRight | OpenFromLeft
	}

	public class IntRange 
	{
		public static IntRange Empty = new IntRange(default(int), default(int), RangeStates.OpenFromBoth, default(int));
		
		private int m_start;
		private int m_end;
		private RangeStates m_rangeStatus = RangeStates.Closed;
		private int m_step;

		public RangeStates RangeStatus
		{
			get { return m_rangeStatus; }	
		}

		public int Step
		{
			get { return m_step; }
		}
		
		public int Start
		{
			get { return m_start; }
		}

		public int End
		{
			get { return m_end; }
		}

		private int RealStart
		{
			get 
			{
				return ((m_rangeStatus & RangeStates.OpenFromLeft) > 0) ? (m_start + m_step) : m_start;
			}
		}

		private int RealEnd
		{
			get 
			{
				return ((m_rangeStatus & RangeStates.OpenFromRight) > 0) ? (m_end - m_step) : m_end;
			}
		}

		protected IntRange()
		{
		}

		public IntRange(int start, int end, int step)
			: this(start, end, RangeStates.Closed, step)
		{
		}

		public IntRange(int start, int end, RangeStates rangeStatus, int step)
		{
			if (start > end)
				throw new ArgumentException("Start interval must by greater than end.");
			m_rangeStatus = rangeStatus;
			m_start = start;
			m_end = end;
			m_step = step;
		}
				
		public override bool Equals(object obj)
		{
			IntRange compareRange = obj as IntRange;
			if (compareRange == null)
				return false;
			if (m_rangeStatus != compareRange.RangeStatus)
				return false;
			if (m_start.Equals(compareRange.Start) && m_end.Equals(compareRange.End))
				return true;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return ((this.Start.ToString().GetHashCode()) + (this.End.ToString().GetHashCode()));
		}
		
		//private bool IsOverlapSecondIsOpen(IntRange first, IntRange second)
		//{
		//    //second je otevreny z leva
		//    if ((int)second.RangeStatus == 0x01)
		//    {
		//        if ((first.Start == second.End) && (first.End == second.Start))
		//            return true;
		//        else
		//            return false;				
		//    }
		//    //second je otevreny z prava
		//    if ((int)second.RangeStatus == 0x02)
		//    {
		//        if ((first.Start < second.End) && (first.End >= second.Start))
		//            return true;
		//        else
		//            return false;
		//    }
		//    //second je otevreny z obou stran
		//    if ((first.Start < second.End) && (first.End > second.Start))
		//        return true;
		//    else
		//        return false;
		//}

		//public bool IsOverlapTest(IntRange other)
		//{
		//    ////oba intervaly jsou uzavrene
		//    if ((this.RangeStatus == RangeStates.Closed) && (other.RangeStatus == RangeStates.Closed))
		//    {
		//        if ((this.Start <= other.End) && (this.End >= other.Start))
		//            return true;
		//        else
		//            return false;
		//    }
		//    //other je otevreny alespon z jedne strany
		//    if ((this.RangeStatus == RangeStates.Closed) && ((int)other.RangeStatus > 0))
		//    {
		//        return IsOverlapSecondIsOpen(this, other);
		//    }
		//    //this je otevreny alespon z jedne strany
		//    if ((other.RangeStatus == RangeStates.Closed) && ((int)this.RangeStatus > 0))
		//    {
		//        return IsOverlapSecondIsOpen(other, this);
		//    }
		//    //oba intervaly jsou otevrene alespon na jedne strane
		//    if (IsOverlapSecondIsOpen(other, this) || IsOverlapSecondIsOpen(this, other))
		//        return true;
		//    else
		//        return false;		
		//}


		public bool IsOverlap(IntRange other)
		{
			if (IsEmpty(other))
				return false;

			if ((this.RealStart <= other.RealEnd) && (this.RealEnd >= other.RealStart))
				return true;
			else
				return false;
		}

		
		public bool Contains(IntRange other)
		{
			if (IsEmpty(other))
				return false;
			if (!IsOverlap(other))
				return false;
			if ((other.RealStart >= this.RealStart) && (other.RealEnd <= this.RealEnd))
				return true;
			else
				return false;
		}

		public bool Contains(int element)
		{
			return (element >= this.RealStart && element <= this.RealEnd);
		}

		public bool Abuts(IntRange other)
		{
			IntRange gap = this.Gap(other);
			return ((IsEmpty(gap)) || ((gap.RealStart - gap.RealEnd) == default(int)));
		}

		public IntRange Gap(IntRange other)
		{
			if (IsEmpty(other))
				return IntRange.Empty;
			if (IsOverlap(other))
				return IntRange.Empty;
			RangeStates overlapStatus = RangeStates.None;
			if (other.RealStart > this.RealStart)
			{
				if ((other.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if ((this.RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if (overlapStatus == RangeStates.None)	
					overlapStatus = RangeStates.Closed;
				return new IntRange(this.End, other.Start, overlapStatus, this.Step);
			}
			else
			{
				if ((this.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if ((other.RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new IntRange(other.End, this.Start, overlapStatus, this.Step);
			}
		}

		public static bool IsEmpty(IntRange range)
		{
			return (((range.Start == range.End) || ((range.End - range.Start) == range.Step))&& range.RangeStatus == RangeStates.OpenFromBoth);
		}

		public IntRange GetOverlap(IntRange other)
		{
			if (IsEmpty(other))
				return IntRange.Empty;
			if (!IsOverlap(other))
				return IntRange.Empty;
			if (!other.IsOverlap(this))
				return IntRange.Empty;
			if (Contains(other))
				return other;
			if (other.Contains(this))
				return this;
			RangeStates overlapStatus = RangeStates.None;
			if (other.RealStart >= this.RealStart)
			{
				if ((other.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if ((this.RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new IntRange(other.Start, this.End, overlapStatus, this.Step);
			}
			else
			{
				if ((this.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if ((other.RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new IntRange(this.Start, other.End, overlapStatus, this.Step);				
			}
		}
		
		public int CompareTo(IntRange other)
		{
			if (!RealStart.Equals(other.RealStart)) return RealStart.CompareTo(other.RealStart);
			return RealEnd.CompareTo(other.RealEnd);				
		}	
	}
}
