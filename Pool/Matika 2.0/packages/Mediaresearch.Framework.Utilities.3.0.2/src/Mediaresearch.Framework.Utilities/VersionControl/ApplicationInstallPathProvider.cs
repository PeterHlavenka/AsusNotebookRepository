using System;

namespace Mediaresearch.Framework.Utilities.VersionControl
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class ApplicationInstallFolderDbIdAttribute : Attribute
	{
		public byte Id { get; set; }

		public ApplicationInstallFolderDbIdAttribute(byte id)
		{
			Id = id;
		}
	}

	public interface IApplicationInstallPathProvider
	{
		string GetAppInstalPath(byte id);
	}
}