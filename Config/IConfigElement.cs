namespace HalfbyteMedia.OnARoll.MakioClient.Config
{
    public enum ConfigType
    {
        Default,
        Internal,
        Engine,
    }

    public enum ConfigBladeClass
    {
        None,
        General,
        Translation
    }


    public interface IConfigElement
    {
        string Name { get; }
        string Description { get; }
        public string BladeClassPropertyName { get; }

        ConfigType ConfigType { get; }
        ConfigBladeClass ConfigBladeClass { get; }

        Type ElementType { get; }

        object BoxedValue { get; set; }
        object DefaultValue { get; }

        object GetLoaderConfigValue();

        void RevertToDefaultValue();

        Action OnValueChangedNotify { get; set; }
    }
}
