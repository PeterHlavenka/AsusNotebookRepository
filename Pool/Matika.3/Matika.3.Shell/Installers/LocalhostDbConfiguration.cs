namespace Matika._3.Shell.Installers
{
    public class LocalhostDbConfiguration : DbAcessConfigurationBase
    {
        public LocalhostDbConfiguration(string localhostDbAlias, string localhostConnectionString, string serverTimeZone)
        {
            LocalhostDbAlias = localhostDbAlias;
            LocalhostConnectionString = localhostConnectionString;
            ServerTimeZone = serverTimeZone;
        }

        protected override string ConnectionString => LocalhostConnectionString;

        public string LocalhostDbAlias { get; }

        public string LocalhostConnectionString { get; }

        public string ServerTimeZone { get; }
    }
}