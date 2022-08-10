
namespace HalfbyteMedia.OnARoll.MakioClient
{
    public class Utils
    {
        public static object GetPrivateProperty<T>(T obj, string properyName)
        {
            return typeof(T).GetField(properyName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

        public static void GetPrivateMethods<T>(T obj, string methodName)
        {
            foreach (var method in obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                MakioClient.LogWarning(method.Name.ToLower());
            }
        }

        public static void SetPrivatePropertyValue<T>(T obj, string propertyName, object newValue)
        {
            foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (fi.Name.ToLower().Contains(propertyName.ToLower()))
                {
                    fi.SetValue(obj, newValue);
                    break;
                }
            }
        }
    }
}
