namespace Mediaresearch.Framework.Utilities.Authentication
{
	public class PathInfo
	{
		public PathInfo(string path, Credentials credentials)
		{
			Path = path;
			Credentials = credentials;
		}

		public string Path { get; private set; }
		public Credentials Credentials { get; private set; }
	}
}
