using BepInEx.Configuration;
using HalfbyteMedia.OnARoll.MakioClient.Config;

namespace HalfbyteMedia.OnARoll.MakioClient.Loader.BepInEx
{
    public class BepInExConfigHandler : ConfigHandler
    {
        private ConfigFile Config => ExplorerBepInPlugin.Instance.Config;

        private const string CTG_NAME = "MakioClient";

        public override void Init()
        {
            // Not necessary
        }

        public override void RegisterConfigElement<T>(ConfigElement<T> config)
        {
            var entry = Config.Bind(CTG_NAME, config.Name, config.Value, config.Description);

            entry.SettingChanged += (object o, EventArgs e) =>
            {
                config.Value = entry.Value;
            };
        }

        public override T GetConfigValue<T>(ConfigElement<T> element)
        {
            if (Config.TryGetEntry(CTG_NAME, element.Name, out ConfigEntry<T> configEntry))
                return configEntry.Value;
            else
                throw new Exception("Could not get config entry '" + element.Name + "'");
        }

        public override void SetConfigValue<T>(ConfigElement<T> element, T value)
        {
            if (Config.TryGetEntry(CTG_NAME, element.Name, out ConfigEntry<T> configEntry))
                configEntry.Value = value;
            else
                MakioClient.Log("Could not get config entry '" + element.Name + "'");
        }

        public override void LoadConfig()
        {
            foreach (var entry in ConfigManager.ConfigElements)
            {
                var key = entry.Key;
                var def = new ConfigDefinition(CTG_NAME, key);
                if (Config.ContainsKey(def) && Config[def] is ConfigEntryBase configEntry)
                {
                    var config = entry.Value;
                    config.BoxedValue = configEntry.BoxedValue;
                }
            }
        }

        public override void SaveConfig()
        {
            // not required
        }
    }
}
