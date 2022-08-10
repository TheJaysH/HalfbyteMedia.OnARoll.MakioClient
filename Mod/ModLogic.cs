using HalfbyteMedia.OnARoll.MakioClient.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalfbyteMedia.OnARoll.MakioClient.Mod
{
    public static class ModLogic
    {
        public static void Update()
        {
            UpdateSlowMo();
        }

        private static void UpdateSlowMo()
        {
            if (Mod.ModPatches.Patch_BladeController.bladeController == null)
                return;

            var bladeController = Mod.ModPatches.Patch_BladeController.bladeController;

            Time.timeScale = ModGlobals.InSlowMo ? EngineConfigHandler.SlowMoTimeScale.Value : bladeController._globalsContainer.TimeProperties.regularTime;
        }
    }
}
