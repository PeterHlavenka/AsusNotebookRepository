namespace Mediaresearch.Framework.Domain.History
{
	public interface IObjectWithValidity
	{
		ValidityRange Validity { get; set; }
	}
}