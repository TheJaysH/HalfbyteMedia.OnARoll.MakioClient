using UniverseLib.UI.Panels;

namespace HalfbyteMedia.OnARoll.MakioClient.UI.Panels
{
    public class PanelDragger : UniverseLib.UI.Panels.PanelDragger
    {
        public PanelDragger(PanelBase uiPanel) : base(uiPanel) { }

        protected override bool MouseInResizeArea(Vector2 mousePos)
        {
            return !UIManager.NavBarRect.rect.Contains(UIManager.NavBarRect.InverseTransformPoint(mousePos))
                && base.MouseInResizeArea(mousePos);
        }
    }
}
