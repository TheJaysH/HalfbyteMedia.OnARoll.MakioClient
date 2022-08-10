
using Persistence.Profile;
using Rewired;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using static HalfbyteMedia.OnARoll.MakioClient.Mod.ModPatches;

namespace HalfbyteMedia.OnARoll.MakioClient.UI.Panels
{
    class DebugPanel : Panel
    {
        public DebugPanel(UIBase owner) : base(owner)
        {
        }

        public override UIManager.Panels PanelType => UIManager.Panels.Debug;
        public override string Name => "Debug";

        public override int MinWidth => 400;

        public override int MinHeight => 320;

        public override Vector2 DefaultAnchorMin => new(0.4f, 0.4f);

        public override Vector2 DefaultAnchorMax => new(0.6f, 0.6f);


        static bool debugValue;
        static Text debugText;

        static Vector3 positionValue;
        static Text positionText;

        static float speedValue;
        static Text speedText;

     

        public override void Update()
        {
            if (Patch_BladeController.bladeController == null)
                return;

            var globalsContainer = Patch_BladeController.bladeController._globalsContainer;
            var translation = globalsContainer.Translation;
            var general = globalsContainer.General;


            if (speedValue != translation.relativeSpeed)
            {
                speedValue = translation.relativeSpeed;
                speedText.text = GetFormattedString("Speed", speedValue);
            }

            if (positionValue != translation.prevPos)
            {
                positionValue = translation.prevPos;
                positionText.text = GetFormattedString("Position", positionValue);
            }

            if (debugValue != general.debug)
            {
                debugValue = general.debug;
                debugText.text = GetFormattedString("DebugEnabled", debugValue);
            }

        }

        public string GetFormattedString(string key, object value)
        {            
            return $"{key}: <color=grey>{value}</color>";
        }

        private Text AddTextElement(string name, string defaultValue)
        {
            var element = UIFactory.CreateLabel(ContentRoot, name, defaultValue, TextAnchor.MiddleLeft, Color.white, true);
            UIFactory.SetLayoutElement(element.gameObject, minWidth: 150, minHeight: 25, flexibleWidth: 9999);
            return element;
        }

        protected override void ConstructPanelContent()
        {
            debugText = AddTextElement("debugText", GetFormattedString("DebugEnabled", false));
            positionText = AddTextElement("positionText", GetFormattedString("Position", 0));
            speedText = AddTextElement("speedText", GetFormattedString("Speed", 0));
        }
    }
}
