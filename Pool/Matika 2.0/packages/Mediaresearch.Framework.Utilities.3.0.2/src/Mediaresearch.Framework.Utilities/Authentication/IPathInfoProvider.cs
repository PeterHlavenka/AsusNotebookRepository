using System.Collections.Generic;

namespace Mediaresearch.Framework.Utilities.Authentication
{
	public interface IPathInfoProvider
	{
		List<PathInfo> GetPathInfos();
		PathInfo GetPathInfo(string path);
	}
}
