using HalfbyteMedia.OnARoll.MakioClient.Config;

namespace HalfbyteMedia.OnARoll.MakioClient.Loader
{
    public interface ILoader
    {
        string CoreModFolderDestination { get; }
        string CoreModFolderName { get; }
        string UnhollowedModulesFolder { get; }

        ConfigHandler ConfigHandler { get; }

        Action<object> OnLogMessage { get; }
        Action<object> OnLogWarning { get; }
        Action<object> OnLogError { get; }
    }
}
