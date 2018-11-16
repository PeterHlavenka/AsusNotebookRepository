namespace Mediaresearch.Framework.Utilities
{
	public interface ICanBeBusy
	{
		bool IsBusy { get; }
	}

	public delegate void IsBusyChangedDelegate(ICanBeBusy canBeBusy, bool isBusy);

	public interface ICanBeBusyNotifier : ICanBeBusy
	{
		event IsBusyChangedDelegate IsBusyChanged;
	}
}