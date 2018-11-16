namespace Mediaresearch.Framework.Utilities.Configuration
{
    public interface IConfigurationProvider
    {
        T GetConfig<T>() where T : ConfigBase;
    }
}
