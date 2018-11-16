
namespace Mediaresearch.Framework.Utilities.VersionControl
{
	public interface IVersionController
	{
		IApplicationInstallPathProvider ApplicationInstallPathProvider { get; set; }
		IPermissionsProvider PermissionsProvider { get; set; }
		/// <summary>
		/// Urci, zda se ma pri kontrole verze informovat o pripadnych novejsich verzich. Vychozi hodnota je true.
		/// </summary>
		bool LookForNewerVersions { get; set; }

		void CheckVersion(out bool supported);

		/// <summary>
		/// Metoda pretezujici <see cref="CheckVersion(out bool)"/>. S out parametrem se neda poradne pracovat pri dispatchovani.
		/// </summary>
		/// <returns>prava, pokud je verze podporovana, jinak nepravda</returns>
		bool CheckVersion();
	}
}
