using Mediaresearch.Framework.Utilities.DataStructures;

namespace Mediaresearch.Framework.Utilities
{
	public class DoubleRange : RangeBase<double, double>
	{
		public DoubleRange()
		{
		}

		public DoubleRange(double start, double end, double step) 
			: base(start, end, step)
		{
		}

		public DoubleRange(double start, double end, RangeStates rangeStatus, double step) 
			: base(start, end, rangeStatus, step)
		{
		}

		public override RangeBase<double, double> Empty
		{
			get { return new DoubleRange(default(double), default(double), RangeStates.OpenFromBoth, default(double)); }
		}

		protected override double AddStep(double intevalBoundary, double step)
		{
			return intevalBoundary + step;
		}

		protected override double SubstractStep(double intervalBoundary, double step)
		{
			return AddStep(intervalBoundary, -step);
		}

		protected override double Gap(double start, double end)
		{
			return end - start;
		}

		protected override double GapInStep(double start, double end)
		{
			return Gap(start, end);
		}

		public override RangeBase<double, double> GetInstance(double start, double end, RangeStates rangeStatus, double step)
		{
			return new DoubleRange(start, end, rangeStatus, step);
		}
	}
}