
using Persistence.Profile;
using Rewired;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using static HalfbyteMedia.OnARoll.MakioClient.Mod.ModPatches;

namespace HalfbyteMedia.OnARoll.MakioClient.UI.Panels
{
    class UtilsPanel : Panel
    {
        public UtilsPanel(UIBase owner) : base(owner)
        {
        }

        public override UIManager.Panels PanelType => UIManager.Panels.Utils;
        public override string Name => "Utils";

        public override int MinWidth => 400;

        public override int MinHeight => 320;

        public override Vector2 DefaultAnchorMin => new(0.4f, 0.4f);

        public override Vector2 DefaultAnchorMax => new(0.6f, 0.6f);


        static ButtonRef reInitControlsButton;
        static ButtonRef reInitCharacterButton;

        protected override void ConstructPanelContent()
        {
            reInitCharacterButton = UIFactory.CreateButton(ContentRoot, "ReInitCharacterButton", "Re-Initialize Character");
            UIFactory.SetLayoutElement(reInitCharacterButton.GameObject, minWidth: 150, minHeight: 25, flexibleWidth: 9999);
            reInitCharacterButton.OnClick += ReInitCharacterButton_OnClick;

            reInitControlsButton = UIFactory.CreateButton(ContentRoot, "ReInitControlsButton", "Re-Initialize Controls");
            UIFactory.SetLayoutElement(reInitControlsButton.GameObject, minWidth: 150, minHeight: 25, flexibleWidth: 9999);
            reInitControlsButton.OnClick += ReInitControlsButton_OnClick;
        }

        private void ReInitControlsButton_OnClick()
        {
            if (Patch_PressAnyButtonController.startMenuController == null)
            {
                MakioClient.LogError("StartMenuController is null");
                return;
            }

            try
            {
                MakioClient.Log("ReInitializing Controls");
                Patch_PressAnyButtonController.player = ReInput.players.GetPlayer(0);
                Patch_PressAnyButtonController.player.controllers.ClearAllControllers();
            }
            catch (Exception ex)
            {
                MakioClient.LogError($"Failed to reinitialize Controls: {ex.Message}");
            }
        }

        private void ReInitCharacterButton_OnClick()
        {
            if (Patch_BladeController.bladeController == null)
            {
                MakioClient.LogError("BladeController is null");
                return;
            }

            var bladeController = Patch_BladeController.bladeController;

            try
            {
                MakioClient.Log("ReInitializing Character");
                bladeController.ReInitialize(ProfilePreferencesManager.Instance.Preferences.CurrentCharacter, ProfilePreferencesManager.Instance.Preferences.CustomCharacterId);
            }
            catch (Exception ex)
            {
                MakioClient.LogError($"Failed to reinitialize character: {ex.Message}");
            }

        }
    }
}
