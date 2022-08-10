using BladeGlobals;
using HalfbyteMedia.OnARoll.MakioClient.Config;
using HarmonyLib;
using Rewired;
using UI.StartMenu;
using UnityEngine.EventSystems;

namespace HalfbyteMedia.OnARoll.MakioClient.Mod
{
    internal class ModPatches
    {
        [HarmonyPatch(typeof(BladeController), "InitCharacter")]
        public class Patch_BladeController
        {
            internal static BladeController bladeController;

            static void Postfix(BladeController __instance)
            {
                Debug.Log("*** Patched BladeController ***");


                bladeController = __instance;
            }
        }

        [HarmonyPatch(typeof(BladeGlobalsContainer), "Initialization")]
        public class Patch_BladeGlobalsContainer
        {
            internal static BladeGlobalsContainer bladeGlobalsContainer;

            static void Postfix(BladeGlobalsContainer __instance)
            {
                Debug.Log("*** Patched BladeGlobalsContainer ***");
                bladeGlobalsContainer = __instance;
            }
        }

        [HarmonyPatch(typeof(StartMenuController), "Update")]
        public class Patch_StartMenuController_Update
        {
            static bool Prefix(ref EventSystem ____currentEventSystem, ref GameObject ____currentSelected, ref GameObject ____secondSelected)
            {
                if (____currentEventSystem != null && ____currentEventSystem.currentSelectedGameObject == null)
                {
                    if (____currentSelected != null && ____secondSelected != null)
                    {
                        ____currentEventSystem.SetSelectedGameObject(____currentSelected);
                        if (!____currentEventSystem.GetComponent<Button>().interactable)
                        {
                            ____currentEventSystem.SetSelectedGameObject(____secondSelected);
                        }
                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(StartMenuController), MethodType.Constructor)]
        public class Patch_PressAnyButtonController
        {
            public static StartMenuController startMenuController;

            public static Player player
            {
                get => (Player)Utils.GetPrivateProperty(startMenuController, "_player");
                set => Utils.SetPrivatePropertyValue(startMenuController, "_player", value);
            }

            static bool Prefix()
            {
                Debug.Log("*** Patched StartMenuController Prefix ***");

                return true;
            }

            static void Postfix(StartMenuController __instance)
            {
                Debug.Log("*** Patched StartMenuController Postfix ***");
                startMenuController = __instance;
            }
        }

        [HarmonyPatch(typeof(General), MethodType.Constructor)]
        public class Patch_General_Constructor
        {
            static bool Prefix(
                ref float ___radius,
                ref float ___minLaunchSpeed,
                ref float ___minCessSlideSpeed,
                ref float ___predictedCycles,
                ref float ___stepSize,
                ref float ___penetrationLimit,
                ref float ___dragCoefficient,
                ref float ___massDensity,
                ref float ___referenceArea,
                ref float ___g,
                ref float ___errorMargin,
                ref bool ___debug,
                ref bool ___isPlayer,
                ref float ___curSquadFactor
                )
            {
                Debug.Log("*** Patched General Prefix ***");
                ___radius = EngineConfigHandler.Radius.Value;            
                ___minLaunchSpeed = EngineConfigHandler.MinLaunchSpeed.Value;
                ___minCessSlideSpeed = EngineConfigHandler.MinCessSlideSpeed.Value;               
                ___stepSize = EngineConfigHandler.StepSize.Value;             
                ___dragCoefficient = EngineConfigHandler.DragCoefficient.Value;    
                ___massDensity = EngineConfigHandler.MassDensity.Value;            
                ___referenceArea = EngineConfigHandler.ReferenceArea.Value;        
                ___g = EngineConfigHandler.Gravity.Value;
                ___errorMargin = EngineConfigHandler.ErrorMargin.Value;
                ___debug = EngineConfigHandler.DebugMode.Value;
                ___isPlayer = false;
                ___curSquadFactor = 0f;

                MakioClient.LogWarning($"___minCessSlideSpeed: {___minCessSlideSpeed}");

                return false;
            }

            static void Postfix(General __instance)
            {
                Debug.Log("*** Patched General Postfix ***");
            }

        }

        [HarmonyPatch(typeof(Translation), MethodType.Constructor, new Type[] { typeof(BladeGlobalsContainer) })]
        public class Patch_Translation_Constructor
        {

            public static Translation translation;

            static bool Prefix(BladeGlobalsContainer globalsContainer, ref BladeGlobalsContainer ___globalsContainer)
            {
                Debug.Log("*** Patched Translation Prefix ***");

                //___globalsContainer = globalsContainer;                

                return true;
            }

            static void Postfix(Translation __instance)
            {
                Debug.Log("*** Patched Translation Postfix ***");
                translation = __instance;
            }
        }

        [HarmonyPatch(typeof(InputMapping), "InitHandlers")]
        public class Patch_InputMapping_InitHandlers
        {
            static void Postfix(InputMapping __instance)
            {
                Debug.Log("*** Patched InputMapping::InitHandlers Prefix ***");



                __instance.AddInputHandler(new Mods.Input.Button4(ButtonID.button4, Patch_BladeController.bladeController));
            }

        }

        [HarmonyPatch(typeof(InAirProperties), MethodType.Constructor)]
        public class Patch_InAirProperties_Constructor
        {

            public static InAirProperties inAirProperties;

            static bool Prefix(
                ref float ___maxJumpPreparationTime,
                ref float ___minJumpPowerFactor,
                ref float ___maxJumpPower,
                ref float ___jumpPower,
                ref float ___jumpDownTime,
                ref float ___jumpUpTime,
                ref float ___impactHeight,
                ref float ___TakeOffPrepStartTime,
                ref float ___highestAirPointRelativeTime,
                ref bool ___adjustLaunchAngle,
                ref float ___initialVerticalAngle,
                ref float ___takeOffTime,
                ref float ___t,
                ref Vector3 ___takeOffPos,
                ref Vector3 ___airFlatUpDir,
                ref Vector3 ___launchDir,
                ref bool ___landingOutSideRamp,
                ref bool ___foundLandingPoint,
                ref bool ___foundFlatLandingPosition,
                ref bool ___airAuto180,
                ref bool ___npcJumpForDestination,
                ref int ___trickDirectionMultiplier,
                ref int ___trickSpinSpeed,
                ref int ___maxFlatSpinSpeed,
                ref int ___maxCorkScrewSpeed,
                ref int ___maxBioSpeed,
                ref int ___maxFlipSpeed,
                ref int ___totalRevolves,
                ref float ___YDegreesSpun,
                ref float ___trickLandingTime,
                ref float ___minLongAirTime,
                ref bool ___didLand
            )
            {
                Debug.Log("*** Patched InAirProperties Prefix ***");

                ___maxJumpPreparationTime = EngineConfigHandler.MaxJumpPreparationTime.Value;
                ___minJumpPowerFactor = EngineConfigHandler.MinJumpPowerFactor.Value; // .55f;
                ___maxJumpPower = EngineConfigHandler.MaxJumpPower.Value; //6f;
                ___jumpPower = EngineConfigHandler.JumpPower.Value;
                ___jumpDownTime = 0f;
                ___jumpUpTime = 0f;
                ___impactHeight = 0f;
                ___TakeOffPrepStartTime = 0f;
                ___highestAirPointRelativeTime = EngineConfigHandler.HighestAirPointRelativeTime.Value;
                ___adjustLaunchAngle = false;
                ___initialVerticalAngle = 0f;
                ___takeOffTime = 0f;
                ___t = 0f;
                ___takeOffPos = Vector3.zero;
                ___airFlatUpDir = Vector3.zero;
                ___launchDir = Vector3.zero;
                ___landingOutSideRamp = false;
                ___foundLandingPoint = false;
                ___foundFlatLandingPosition = false;
                ___airAuto180 = false;
                ___npcJumpForDestination = false;
                ___trickDirectionMultiplier = 1;
                ___trickSpinSpeed = EngineConfigHandler.TrickSpinSpeed.Value;
                ___maxFlatSpinSpeed = EngineConfigHandler.MaxFlatSpinSpeed.Value;
                ___maxCorkScrewSpeed = EngineConfigHandler.MaxCorkScrewSpeed.Value;
                ___maxBioSpeed = EngineConfigHandler.MaxBioSpeed.Value;
                ___maxFlipSpeed = EngineConfigHandler.MaxFlipSpeed.Value;
                ___totalRevolves = 0;
                ___YDegreesSpun = 0f;
                ___trickLandingTime = EngineConfigHandler.TrickLandingTime.Value;
                ___minLongAirTime = EngineConfigHandler.MinLongAirTime.Value;
                ___didLand = false;

                return false;
            }

            static void Postfix(InAirProperties __instance)
            {
                Debug.Log("*** Patched InAirProperties Postfix ***");
                inAirProperties = __instance;

                //inAirProperties.jumpPower = 20f;
            }
        }


    }
}
