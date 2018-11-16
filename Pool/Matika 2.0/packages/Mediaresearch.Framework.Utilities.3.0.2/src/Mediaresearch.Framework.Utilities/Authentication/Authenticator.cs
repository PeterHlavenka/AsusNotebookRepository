using System;
using System.IO;
using Mediaresearch.Framework.Utilities.IO;
using log4net;

namespace Mediaresearch.Framework.Utilities.Authentication
{
	public enum AuthenticationStatus
	{
		Success = 1,
		PathInfoNotFound = 2,
		FailedToAuthenticateWithGivenCredentials = 3,
		UnknownFailure = 0
	}

	public class Authenticator
	{
		private static readonly ILog m_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IPathInfoProvider PathInfoProvider { get; set; }

		public Authenticator(IPathInfoProvider pathInfoProvider)
		{
			PathInfoProvider = pathInfoProvider;
		}

		public AuthenticationStatus TryAuthenticate(string path)
		{
			try
			{
				lock (this)
				{
					PathInfo pathInfo = PathInfoProvider.GetPathInfo(path);
					if (pathInfo == null)
						return AuthenticationStatus.PathInfoNotFound;
					if (IoManager.HasUserRightsToReadWrite(pathInfo.Path))
						return AuthenticationStatus.Success;
					IoManager.UnsetAuthentication(pathInfo.Path);
					bool success = IoManager.SetDevicelessAuthentication(pathInfo.Path, pathInfo.Credentials == null ? string.Empty : pathInfo.Credentials.Username, pathInfo.Credentials == null ? string.Empty : pathInfo.Credentials.Password);
					return success ? AuthenticationStatus.Success : AuthenticationStatus.FailedToAuthenticateWithGivenCredentials;
				}
			}
			catch (Exception ex)
			{
				if (m_log.IsErrorEnabled)
					m_log.Error(string.Format("Unable to authenticate to path {0}!", path), ex);
				return AuthenticationStatus.UnknownFailure;
			}
		}
	}
}