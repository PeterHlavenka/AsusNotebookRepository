using System.Text.RegularExpressions;

namespace Matika._3.Shell.Installers
{
    public abstract class DbAcessConfigurationBase
    {
        protected abstract string ConnectionString { get; }

        public string DefaultDatabaseName
        {
            get
            {
                var match = GetConnectionStringMatch();
                if (match == null || match.Groups.Count < 3)
                    return null;

                return match.Groups[2].Value;
            }
        }

        public string DefaultDataSource
        {
            get
            {
                var match = GetConnectionStringMatch();
                if (match == null || match.Groups.Count < 3)
                    return null;

                return $"{match.Groups[1].Value}/{match.Groups[2].Value}";
            }
        }

        private Match GetConnectionStringMatch()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                return null;

            var regex = new Regex(@".*Data Source=(.*);.*Initial Catalog=([a-z0-9\w\.]*);.*", RegexOptions.IgnoreCase);
            var match = regex.Match(ConnectionString);
            if (!match.Success)
                return null;

            return match;
        }
    }
}