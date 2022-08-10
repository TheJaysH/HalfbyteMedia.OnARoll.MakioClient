
namespace HalfbyteMedia.OnARoll.MakioClient.Runtime
{
    public abstract class RuntimeHelper
    {
        public static RuntimeHelper Instance;

        public static void Init()
        {
            Instance = new MonoHelper();

            Instance.SetupEvents();
        }

        public abstract void SetupEvents();

        private static readonly HashSet<string> currentBlacklist = new();

        public virtual string[] DefaultReflectionBlacklist => new string[0];

    }
}
