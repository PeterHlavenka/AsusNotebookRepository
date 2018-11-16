using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Mediaresearch.Framework.Utilities.DataStructures
{
	[Serializable]
	[DataContract]
    public class DateTimeRange : IComparable
	{		
		public static readonly DateTimeRange Empty = new DateTimeRange(default(DateTime), default(DateTime), RangeStates.OpenFromBoth, new TimeSpan(0,0,1));			
		
		#region Members
		[DataMember]
		private DateTime m_From;
		[DataMember]
		private DateTime m_To;
		[DataMember]
		protected RangeStates m_rangeStatus = RangeStates.Closed;
		[DataMember]
		protected TimeSpan m_step;
		#endregion

		#region Properties
		
		[BrowsableAttribute(false)]
		public RangeStates RangeStatus
		{
			get { return m_rangeStatus; }
		}
		
		[BrowsableAttribute(false)]
		public TimeSpan Step
		{
			get { return m_step; }
		}

		[CategoryAttribute("Platnost modelu"), Description("Od kdy je model platný."), ReadOnlyAttribute(true)]
		public DateTime From
		{
			get { return m_From; }
		}

		[CategoryAttribute("Platnost modelu"), Description("Do kdy je(byl) model platný."), ReadOnlyAttribute(true)]
		public DateTime To
		{
			get { return m_To; }
		}

		private DateTime RealFrom
		{
			get { return ((m_rangeStatus & RangeStates.OpenFromLeft) > 0) ? (m_From + m_step) : m_From; }
		}

		private DateTime RealTo
		{
			get { return ((m_rangeStatus & RangeStates.OpenFromRight) > 0) ? (m_To - m_step) : m_To; }
		}
		#endregion

		#region Constructors
		protected DateTimeRange()
		{			
		}

		public DateTimeRange(DateTime start, DateTime end, TimeSpan step)
			: this(start, end, RangeStates.Closed, step)
		{
		}


		public DateTimeRange(DateTime start, DateTime end, RangeStates rangeStatus, TimeSpan step)
		{
			if (start > end)
				throw new ArgumentException("Start interval must by greater than end.");
			if (step.Ticks <= 0)
				throw new ArgumentException("Range step must not be a zero.");

			m_rangeStatus = rangeStatus;
			m_step = step;
			m_From = RoundToStep(start, m_step);
			m_To = RoundToStep(end, m_step);
		}
		#endregion

		//public override int GetHashCode()
        //{
        //    int result = m_From.GetHashCode();
        //    result = 29*result + m_To.GetHashCode();
        //    result = 29*result + m_rangeStatus.GetHashCode();
        //    result = 29*result + m_step.GetHashCode();
        //    return result;
        //}
        
		public override bool Equals(object obj)
		{
			if (this == obj) return true;
			DateTimeRange dateTimeRange = obj as DateTimeRange;
			if (dateTimeRange == null) return false;
			if (!Equals(m_From, dateTimeRange.m_From)) return false;
			if (!Equals(m_To, dateTimeRange.m_To)) return false;
			if (!Equals(m_rangeStatus, dateTimeRange.m_rangeStatus)) return false;
			if (!Equals(m_step, dateTimeRange.m_step)) return false;
			return true;
		}

	    public override int GetHashCode()
	    {
            // Tohle vrací to samé, jako default implementace object.GetHashCode() pro referenèní typy, ale 
            // bez warningu pøi pøekladu. 
            // Proè pùvodní metodu nìkdo zakomentoval, a WTF neøeším...
            return RuntimeHelpers.GetHashCode(this);
	    }


	    public bool IsOverlap(DateTimeRange other)
		{
			if (IsEmpty(other))
				return false;

			if ((RealFrom <= other.RealTo) && (RealTo >= other.RealFrom))
				return true;
			else
				return false;
		}


		public bool Contains(DateTimeRange other)
		{
			if (IsEmpty(other))
				return false;
			if (!IsOverlap(other))
				return false;
			if ((other.RealFrom >= RealFrom) && (other.RealTo <= RealTo))
				return true;
			else
				return false;
		}

		public virtual bool Contains(DateTime element)
		{
			return (element >= RealFrom && element <= RealTo);
		}

		public bool Abuts(DateTimeRange other)
		{
			DateTimeRange gap = Gap(other);
			return ((IsEmpty(gap)) || ((gap.RealFrom - gap.RealTo) == default(TimeSpan)));
		}

		public DateTimeRange Gap(DateTimeRange other)
		{
			if (IsEmpty(other))
				return Empty;
			if (IsOverlap(other))
				return Empty;
			RangeStates overlapStatus = RangeStates.None;
			if (other.RealFrom > RealFrom)
			{
				if ((other.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if ((RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new DateTimeRange(To, other.From, overlapStatus, Step);
			}
			else
			{
				if ((RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if ((other.RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new DateTimeRange(other.To, From, overlapStatus, Step);
			}
		}

		public DateTimeRange GetOverlap(DateTimeRange other)
		{
			if (IsEmpty(other))
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
			if (other.RealFrom >= RealFrom)
			{
				if ((other.RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if ((RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new DateTimeRange(other.From, To, overlapStatus, Step);
			}
			else
			{
				if ((RangeStatus & RangeStates.OpenFromLeft) > 0)
					overlapStatus |= RangeStates.OpenFromLeft;
				if ((other.RangeStatus & RangeStates.OpenFromRight) > 0)
					overlapStatus |= RangeStates.OpenFromRight;
				if (overlapStatus == RangeStates.None)
					overlapStatus = RangeStates.Closed;
				return new DateTimeRange(From, other.To, overlapStatus, Step);
			}
		}

		public int CompareTo(DateTimeRange other)
		{
			if (!RealFrom.Equals(other.RealFrom)) return RealFrom.CompareTo(other.RealFrom);
			return RealTo.CompareTo(other.RealTo);
		}
		
		public int CompareTo(object obj)
		{
			if (obj is DateTimeRange)
			{
				DateTimeRange temp = (DateTimeRange)obj;
				return m_From.CompareTo(temp.From);
			}
			throw new ArgumentException("CompareTo: object is not a DateTimeRange"); 
		}
		#region Static members

		public static DateTime RoundToStep(DateTime date, TimeSpan step)
		{
			return new DateTime((long) (Math.Round( ((decimal)date.Ticks) / step.Ticks) * step.Ticks));
		}

		public static bool IsEmpty(DateTimeRange range)
		{
			return
				(((range.From == range.To) || ((range.To - range.From) == range.Step)) &&
				 range.RangeStatus == RangeStates.OpenFromBoth);
		}

		#endregion
	}
}
