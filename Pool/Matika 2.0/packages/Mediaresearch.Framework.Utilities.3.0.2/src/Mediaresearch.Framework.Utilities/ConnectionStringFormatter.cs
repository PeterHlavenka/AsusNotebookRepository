using System.Text.RegularExpressions;

namespace Mediaresearch.Framework.Utilities
{
    public static class ConnectionStringFormatter
    {
        /// <summary>
        /// Vysosej server a db z ConnStringu EntityFrameworku
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string FormatConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return null;

            var regex = new Regex(@".*Data Source=(.*);.*Initial Catalog=([^;]*);.*", RegexOptions.IgnoreCase);
            Match match = regex.Match(connectionString);
            if (!match.Success)
                return null;

            return string.Format("{0}-{1}", match.Groups[1].Value, match.Groups[2].Value);
        }
    }
}
