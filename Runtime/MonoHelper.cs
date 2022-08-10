namespace HalfbyteMedia.OnARoll.MakioClient.Runtime
{
    public class MonoHelper : RuntimeHelper
    {
        public override void SetupEvents()
        {
            Application.logMessageReceived += Application_logMessageReceived;
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
             => MakioClient.LogUnity(condition, type);
    }
}
