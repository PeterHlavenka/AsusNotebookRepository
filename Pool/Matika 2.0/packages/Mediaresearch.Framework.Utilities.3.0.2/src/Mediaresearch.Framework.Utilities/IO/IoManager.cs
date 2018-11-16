using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using log4net;

namespace Mediaresearch.Framework.Utilities.IO
{
	public static class IoManager
	{
		private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static bool UnsetAuthentication(string deviceOrPath)
		{
			const string appName = "NET";
			string agrs = string.Format(" USE {0} /delete /y", deviceOrPath);
			if (m_log.IsDebugEnabled)
				m_log.DebugFormat("Unsetting authentication: {0} {1}", appName, agrs);
			ProcessStartInfo psi = new ProcessStartInfo(appName, agrs)
			{
				RedirectStandardOutput = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			var listFiles = Process.Start(psi);
			var myOutput = listFiles.StandardOutput;
			listFiles.WaitForExit();
			if (listFiles.HasExited)
			{
				var output = myOutput.ReadToEnd();
				if (m_log.IsDebugEnabled)
					m_log.DebugFormat("Authentication output was: {0}", output);
				// co cesky wokna?
				if (output.Contains("was deleted successfully"))
				{
					if (m_log.IsDebugEnabled)
						m_log.Debug("Returning true");
					return true;
				}
			}
			if (m_log.IsDebugEnabled)
				m_log.Debug("Returning false");
			return false;
		}

		public static bool SetDevicelessAuthentication(string path, string user, string pswd)
		{
			//net use G: \\Fvs-dc\share pass /user:fvs /Y
			//net use q: \\192.168.25.25\tv-storage bru83qu-a9wruW /user:GrabSys
			const string appName = "NET";
			string agrs = string.Format(" USE {0} {1} {2}{3}", path, pswd, string.IsNullOrEmpty(user) ? string.Empty : "/user:", user);
			if (m_log.IsDebugEnabled)
				m_log.DebugFormat("Authenticating deviceless: {0} {1}", appName, agrs);
			var psi = new ProcessStartInfo(appName, agrs)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			Process listFiles = Process.Start(psi);
			StreamReader myOutput = listFiles.StandardOutput;
			StreamReader errorOutput = listFiles.StandardError;
			listFiles.WaitForExit();
			if (listFiles.HasExited)
			{
				var output = myOutput.ReadToEnd();
				var error = errorOutput.ReadToEnd();
				if (m_log.IsDebugEnabled)
					m_log.DebugFormat("Authentication output was: {0}", output);
				//druha podminka kvuli tomu, ze me nezajima, pokud uz je jiz pripojeno -> uzivatel se tam dostane a navic to ma namapovane pres jednotku
				//if (output.Contains("The command completed successfully.") || error.Contains("Multiple connections to a server"))
				if (HasUserRightsToReadWrite(path))
				{
					if (m_log.IsDebugEnabled)
						m_log.Debug("Returning true");
					return true;
				}
			}
			if (m_log.IsDebugEnabled)
				m_log.Debug("Returning false");
			return false;
		}


		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetShortPathName(
			[MarshalAs(UnmanagedType.LPTStr)] string path,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
			int shortPathLength);

		public static string GetDosPathName(string path)
		{
			var shortPath = new StringBuilder(500);
			if (0 == GetShortPathName(path, shortPath, shortPath.Capacity))
			{
				if (Marshal.GetLastWin32Error() == 2)
				{
					throw new Exception("Short name generation failed!");
				}
				throw new Exception(Marshal.GetLastWin32Error().ToString());
			}
			return shortPath.ToString();
		}

		/// <summary>
		/// Vytvori slozku definovanou cestou path. POZOR - navratova hodnota nesouvisi s uspesnym vytvorenim slozky - tato metoda nezachycuje vyjimky
		/// </summary>
		/// <param name="path">vytvorena slozka bude mit cestu path</param>
		/// <returns>pravda, pokud jiz slozka exitovala, jinak nepravda</returns>
		public static bool CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Smaze vsechny soubory ve slozce directoryPath
		/// </summary>
		/// <param name="directoryPath">cesta ke slozce</param>
		public static void DeleteAllFilesInDirectory(string directoryPath)
		{
			if (!Directory.Exists(directoryPath)) return;

			var files = Directory.GetFiles(directoryPath);
			foreach (var file in files)
			{
				try
				{
					DeleteFile(file);
				}
				catch (IOException)
				{
					//ArrayList a = getFileProcesses(file);
				}
				catch (UnauthorizedAccessException)
				{
					//ArrayList a = getFileProcesses(file);
				}
			}
		}

		/// <summary>
		/// Smaze vsechny soubory a podslozky i se soubory!
		/// </summary>
		/// <param name="directoryPath">cesta ke slozce</param>
		public static void DeleteDirectory(string directoryPath)
		{
			if (!Directory.Exists(directoryPath)) return;

			var files = Directory.GetFiles(directoryPath);
			foreach (var file in files)
			{
				try
				{
					DeleteFile(file);
				}
				catch (IOException)
				{
					//ArrayList a = getFileProcesses(file);
				}
				catch (UnauthorizedAccessException)
				{
					//ArrayList a = getFileProcesses(file);
				}
			}

			foreach (string subdirectory in Directory.GetDirectories(directoryPath))
			{
				DeleteDirectory(subdirectory);
				try
				{
					Directory.Delete(subdirectory);
				}
				catch (IOException)
				{

				}
				catch (UnauthorizedAccessException)
				{

				}
			}

			try
			{
				Directory.Delete(directoryPath);
			}
			catch (IOException)
			{

			}
			catch (UnauthorizedAccessException)
			{

			}
		}

		public static void DeleteFile(string path)
		{
			File.Delete(path);
		}

		public static bool HasUserRightsToReadWrite(string path)
		{
			AuthorizationRuleCollection rules;
			try
			{
				rules = Directory.GetAccessControl(path)
					.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
			}
			catch (Exception)
			{ //Posible UnauthorizedAccessException
				return false;
			}

			var rulesCast = rules.Cast<FileSystemAccessRule>().ToArray();
			if (rulesCast.Any(rule => rule.AccessControlType == AccessControlType.Deny && ((rule.FileSystemRights & (FileSystemRights.Read | FileSystemRights.Write)) == (FileSystemRights.Read | FileSystemRights.Write)))
				|| !rulesCast.Any(rule => rule.AccessControlType == AccessControlType.Allow && ((rule.FileSystemRights & (FileSystemRights.Read | FileSystemRights.Write)) == (FileSystemRights.Read | FileSystemRights.Write))))
			{
				return false;
			}

			return true;
		}

		///<summary>
		/// Ziska z relativni ci absolutni cesty absolutni cestu.
		///</summary>
		///<param name="path">Relativni nebo absolutni cesta</param>
		///<returns>Absolutni cestu nebo null, pokud soubor nemuze byt nalezen jak na absolutni ceste, tak relativni</returns>
		public static string GetFileAbsolutePath(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("Argument path can not be null or empty!");

			if (Path.IsPathRooted(path) && File.Exists(path))
				return path;	//cesta jiz je absolutni a existuje -> vrat ji
			string executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			if (executingAssemblyLocation == null)
				throw new InvalidOperationException("Unable to find out executing assembly location!");
			string executingAssemblyDirectoryName = Path.GetDirectoryName(executingAssemblyLocation);
			if (executingAssemblyDirectoryName == null)
				throw new InvalidOperationException("Unable to find out executing assembly directory name!");
			string absolutePath = Path.Combine(executingAssemblyDirectoryName, path);
			return File.Exists(absolutePath) ? absolutePath : null;
		}
	}
}