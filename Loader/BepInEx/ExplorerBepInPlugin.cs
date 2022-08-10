using BepInEx;
using BepInEx.Logging;
using HalfbyteMedia.OnARoll.MakioClient.Config;
using HarmonyLib;

namespace HalfbyteMedia.OnARoll.MakioClient.Loader.BepInEx
{
    [BepInPlugin(MakioClient.GUID, MakioClient.NAME, MakioClient.VERSION)]
    public class ExplorerBepInPlugin : BaseUnityPlugin, ILoader
    {
        public static ExplorerBepInPlugin Instance;

        public ManualLogSource LogSource => Logger;


        public string UnhollowedModulesFolder => Path.Combine(Paths.BepInExRootPath, "unhollowed");

        public ConfigHandler ConfigHandler => _configHandler;
        private BepInExConfigHandler _configHandler;

        public Harmony HarmonyInstance => s_harmony;
        private static readonly Harmony s_harmony = new(MakioClient.GUID);

        public string CoreModFolderName => MakioClient.DEFAULT_EXPLORER_FOLDER_NAME;
        public string CoreModFolderDestination => Paths.PluginPath;

        public Action<object> OnLogMessage => LogSource.LogMessage;
        public Action<object> OnLogWarning => LogSource.LogWarning;
        public Action<object> OnLogError => LogSource.LogError;

        private void Init()
        {
            Instance = this;
            _configHandler = new BepInExConfigHandler();
            MakioClient.Init(this);
        }

        internal void Awake()
        {
            Init();
        }

    }
}
