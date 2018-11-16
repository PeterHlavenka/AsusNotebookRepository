using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using Mediaresearch.Framework.Utilities.Resources.Localisation;

namespace Mediaresearch.Framework.Utilities.VersionControl
{
	public class VersionController : IVersionController
	{
		public IPermissionsProvider PermissionsProvider { get; set; }
		public IApplicationInstallPathProvider ApplicationInstallPathProvider { get; set; }
		private readonly string m_name, m_appInstallPath;
		private readonly Version m_version;

		public VersionController(IApplicationInstallPathProvider applicationInstallPathProvider, IPermissionsProvider permissionsProvider)
		{
			ApplicationInstallPathProvider = applicationInstallPathProvider;
			PermissionsProvider = permissionsProvider;

			AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
			m_version = assemblyName.Version;
			m_name = assemblyName.Name;

			LookForNewerVersions = true;

			var idAttribute = (ApplicationInstallFolderDbIdAttribute)Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(ApplicationInstallFolderDbIdAttribute));
			if (idAttribute == null)
				throw new Exception("Application's assembly must have ApplicationInstallFolderDbIdAttribute specified!");

			m_appInstallPath = ApplicationInstallPathProvider.GetAppInstalPath(idAttribute.Id);
		}

		public bool LookForNewerVersions { get; set; }

		public void CheckVersion(out bool supported)
		{
			if (PermissionsProvider == null)
				throw new Exception("No PermissionsProvider provider has been set!");
			if (ApplicationInstallPathProvider == null)
				throw new Exception("No ApplicationInstallPathProvider has been set!");

			UserPermissionWrapper[] permissionsWrapper = PermissionsProvider.GetUserPersmissions();

			if (permissionsWrapper.Length <= 0)
				throw new Exception("No supported version found!");

			supported = permissionsWrapper.Any(p => p.Name.Contains(m_version.ToString()));

			Version[] versions = permissionsWrapper.Select(p => GetVersion(p.Name)).ToArray();

			if (supported && LookForNewerVersions && versions.Any(v => v > m_version) &&
				MessageBox.Show(Localisation.NewerVersionExistsQuestion, Localisation.NewerVersion, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{

				DisplayVersionDialog(Localisation.NewerVersion, Localisation.NewerVersionsList, versions.Where(v => v > m_version));

				supported = false;
				return;
			}

			if (!supported)
				DisplayVersionDialog(Localisation.NotSupportedVersion, Localisation.NotSupportedVersionMessage, versions);
		}

		public bool CheckVersion()
		{
			bool isSupported;
			CheckVersion(out isSupported);
			return isSupported;
		}

		private void DisplayVersionDialog(string title, string message, IEnumerable<Version> versions)
		{
			List<LinkHolder> linkHolders = new List<LinkHolder>();

			foreach (var version in versions)
			{
				string path = Path.Combine(m_appInstallPath, string.Format("{0}_{1}.exe", m_name, version));
				linkHolders.Add(new LinkHolder { Path = path, Command = new NavigateCommand { Path = path } });
			}

			VersionDialogView view = new VersionDialogView();
			VersionDialogViewModel model = new VersionDialogViewModel { Holders = linkHolders };
			model.DialogTitle = title;
			model.Message = message;
			view.DataContext = model;
			view.ShowDialog();
		}

		private Version GetVersion(string fullName)
		{
			Regex versionPattern = new Regex(@"\d+.\d+.\d+.\d+.");
			Match result = versionPattern.Match(fullName);
			return new Version(result.ToString());
		}
	}
}