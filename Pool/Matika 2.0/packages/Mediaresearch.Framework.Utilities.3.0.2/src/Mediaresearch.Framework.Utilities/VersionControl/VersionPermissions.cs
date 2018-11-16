namespace Mediaresearch.Framework.Utilities.VersionControl
{
	public class UserPermissionWrapper
	{
		public string Name { get; set; }
	}

	public interface IPermissionsProvider
	{
		UserPermissionWrapper[] GetUserPersmissions();
	}
}