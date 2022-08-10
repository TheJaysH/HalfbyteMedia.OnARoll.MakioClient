using UniverseLib.UI.Panels;

namespace HalfbyteMedia.OnARoll.MakioClient.UI
{
    public class UIBase : UniverseLib.UI.UIBase
    {
        public UIBase(string id, Action updateMethod) : base(id, updateMethod) { }

        protected override PanelManager CreatePanelManager()
        {
            return new UEPanelManager(this);
        }
    }
}
