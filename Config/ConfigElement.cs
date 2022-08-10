namespace HalfbyteMedia.OnARoll.MakioClient.Config
{

    public class ConfigElement<T> : IConfigElement
    {
        public string Name { get; }
        public string Description { get; }

        public string BladeClassPropertyName { get; }

        public ConfigType ConfigType { get; }
        public ConfigBladeClass ConfigBladeClass { get; }

        public Type ElementType => typeof(T);

        public Action<T> OnValueChanged;
        public Action OnValueChangedNotify { get; set; }

        public object DefaultValue { get; }

  

        public ConfigHandler Handler
        {
            get
            {
                return ConfigType switch
                {
                    ConfigType.Default => ConfigManager.Handler,
                    ConfigType.Internal => ConfigManager.InternalHandler,
                    ConfigType.Engine => ConfigManager.EngineHandler,
                    _ => throw new NotImplementedException(),
                };              
            }
        }

        public T Value
        {
            get => m_value;
            set => SetValue(value);
        }
        private T m_value;

        object IConfigElement.BoxedValue
        {
            get => m_value;
            set => SetValue((T)value);
        }

        public ConfigElement(string name, string description, T defaultValue, ConfigType configType = ConfigType.Default, ConfigBladeClass configBladeClass = ConfigBladeClass.None, string bladeClassPropertyName = ""/*, bool isInternal = false, bool isEngine = false */)
        {
            Name = name;
            Description = description;

            BladeClassPropertyName = string.IsNullOrEmpty(bladeClassPropertyName) && configType == ConfigType.Engine ? name.ToLower() : bladeClassPropertyName;

            m_value = defaultValue;
            DefaultValue = defaultValue;

            ConfigType = configType;
            ConfigBladeClass = configBladeClass;


            //IsInternal = isInternal;
            //IsEngine = isEngine;

            ConfigManager.RegisterConfigElement(this);
        }

        private void SetValue(T value)
        {
            if ((m_value == null && value == null) || (m_value != null && m_value.Equals(value)))
                return;

            m_value = value;

            Handler.SetConfigValue(this, value);

            OnValueChanged?.Invoke(value);
            OnValueChangedNotify?.Invoke();

            Handler.OnAnyConfigChanged();
        }

        object IConfigElement.GetLoaderConfigValue() => GetLoaderConfigValue();

        public T GetLoaderConfigValue()
        {
            return Handler.GetConfigValue(this);
        }

        public void RevertToDefaultValue()
        {
            Value = (T)DefaultValue;
        }
    }
}
