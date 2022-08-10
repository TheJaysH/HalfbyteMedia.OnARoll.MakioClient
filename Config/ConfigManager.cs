using HalfbyteMedia.OnARoll.MakioClient.UI;

namespace HalfbyteMedia.OnARoll.MakioClient.Config
{
    public static class ConfigManager
    {
        internal static readonly Dictionary<string, IConfigElement> ConfigElements = new();
        internal static readonly Dictionary<string, IConfigElement> InternalConfigs = new();
        internal static readonly Dictionary<string, IConfigElement> EgnineConfigs = new();

        // Each Mod Loader has its own ConfigHandler.
        // See the MakioClient.Loader namespace for the implementations.
        public static ConfigHandler Handler { get; private set; }

        // Actual UE Settings
        public static ConfigElement<KeyCode> Master_Toggle;
        public static ConfigElement<bool> Hide_On_Startup;
        //public static ConfigElement<float> Startup_Delay_Time;
        //public static ConfigElement<bool> Disable_EventSystem_Override;
        public static ConfigElement<int> Target_Display;
        //public static ConfigElement<bool> Force_Unlock_Mouse;
        //public static ConfigElement<KeyCode> Force_Unlock_Toggle;
        //public static ConfigElement<string> Default_Output_Path;
        //public static ConfigElement<bool> Log_Unity_Debug;
        //public static ConfigElement<UIManager.VerticalAnchor> Main_Navbar_Anchor;

        // internal configs
        internal static InternalConfigHandler InternalHandler { get; private set; }
        internal static EngineConfigHandler EngineHandler { get; private set; }

        internal static readonly Dictionary<UIManager.Panels, ConfigElement<string>> PanelSaveData = new();
        //internal static readonly Dictionary<string, ConfigElement<float>> EngineSaveData = new();

        internal static ConfigElement<string> GetPanelSaveData(UIManager.Panels panel)
        {
            if (!PanelSaveData.ContainsKey(panel))
                PanelSaveData.Add(panel, new ConfigElement<string>(panel.ToString(), string.Empty, string.Empty, ConfigType.Internal));
            return PanelSaveData[panel];
        }

        //internal static ConfigElement<float> GetEngineSaveData(string key)
        //{
        //    if (!EngineSaveData.ContainsKey(key))
        //        EngineSaveData.Add(key, new ConfigElement<float>(key, string.Empty, 0f, ConfigType.Engine));

        //    return EngineSaveData[key];
        //}

        public static void Init(ConfigHandler configHandler)
        {
            Handler = configHandler;
            Handler.Init();

            InternalHandler = new InternalConfigHandler();
            InternalHandler.Init();

            EngineHandler = new EngineConfigHandler();
            EngineHandler.Init();
            EngineHandler.CreateConfigElements();

            CreateConfigElements();

            Handler.LoadConfig();
            InternalHandler.LoadConfig();
            EngineHandler.LoadConfig();
        }

        internal static void RegisterConfigElement<T>(ConfigElement<T> configElement)
        {

            switch (configElement.ConfigType)
            {
                case ConfigType.Default:
                    Handler.RegisterConfigElement(configElement);
                    ConfigElements.Add(configElement.Name, configElement);
                    break;
                case ConfigType.Internal:
                    InternalHandler.RegisterConfigElement(configElement);
                    InternalConfigs.Add(configElement.Name, configElement);
                    break;
                case ConfigType.Engine:
                    EngineHandler.RegisterConfigElement(configElement);
                    EgnineConfigs.Add(configElement.Name, configElement);
                    MakioClient.Log($"Adding configElement: {configElement.Name}");
                    break;
                default:
                    break;
            }

        }

        private static void CreateConfigElements()
        {

            Master_Toggle = new("Makio Client Toggle",
                "The key to enable or disable MakioClient's menu and features.",
                KeyCode.F7);

            Hide_On_Startup = new("Hide On Startup",
                "Should MakioClient be hidden on startup?",
                true);

            //Startup_Delay_Time = new("Startup Delay Time",
            //    "The delay on startup before the UI is created.",
            //    1f);

            Target_Display = new("Target Display",
                "The monitor index for MakioClient to use, if you have multiple. 0 is the default display, 1 is secondary, etc. " +
                "Restart recommended when changing this setting. Make sure your extra monitors are the same resolution as your primary monitor.",
                0);

            //Force_Unlock_Mouse = new("Force Unlock Mouse",
            //    "Force the Cursor to be unlocked (visible) when the MakioClient menu is open.",
            //    true);
            //Force_Unlock_Mouse.OnValueChanged += (bool value) => UniverseLib.Config.ConfigManager.Force_Unlock_Mouse = value;

            //Force_Unlock_Toggle = new("Force Unlock Toggle Key",
            //    "The keybind to toggle the 'Force Unlock Mouse' setting. Only usable when MakioClient is open.",
            //    KeyCode.None);

            //Default_Output_Path = new("Default Output Path",
            //   "The default output path when exporting things from MakioClient.",
            //   Path.Combine(MakioClient.CoreModFolder, "Output"));

            //Disable_EventSystem_Override = new("Disable EventSystem override",
            //    "If enabled, MakioClient will not override the EventSystem from the game.\n<b>May require restart to take effect.</b>",
            //    false);
            //Disable_EventSystem_Override.OnValueChanged += (bool value) => UniverseLib.Config.ConfigManager.Disable_EventSystem_Override = value;



            //Main_Navbar_Anchor = new("Main Navbar Anchor",
            //    "The vertical anchor of the main MakioClient Navbar, in case you want to move it.",
            //    UIManager.VerticalAnchor.Top);

        }
    }
}
