using BladeGlobals;
using HalfbyteMedia.OnARoll.MakioClient.UI;
using Tomlet;
using Tomlet.Models;

namespace HalfbyteMedia.OnARoll.MakioClient.Config
{
    public class EngineConfigHandler : ConfigHandler
    {
        private static string CONFIG_PATH;


        public static ConfigElement<float> SlowMoTimeScale;
        public static ConfigElement<float> Radius;
        public static ConfigElement<float> MinLaunchSpeed;
        public static ConfigElement<float> MinCessSlideSpeed;
        public static ConfigElement<float> StepSize;
        public static ConfigElement<float> DragCoefficient;
        public static ConfigElement<float> MassDensity;
        public static ConfigElement<float> ReferenceArea;
        public static ConfigElement<float> Gravity;
        public static ConfigElement<float> ErrorMargin;
        public static ConfigElement<bool>  DebugMode;

        public static ConfigElement<float> MaxJumpPreparationTime;
        public static ConfigElement<float> MinJumpPowerFactor;
        public static ConfigElement<float> MaxJumpPower;
        public static ConfigElement<float> JumpPower;
        public static ConfigElement<float> HighestAirPointRelativeTime;
        public static ConfigElement<int> TrickSpinSpeed;
        public static ConfigElement<int> MaxFlatSpinSpeed;
        public static ConfigElement<int> MaxCorkScrewSpeed;
        public static ConfigElement<int> MaxBioSpeed;
        public static ConfigElement<int> MaxFlipSpeed;
        public static ConfigElement<float> TrickLandingTime;
        public static ConfigElement<float> MinLongAirTime;


        public override void Init()
        {            
            CONFIG_PATH = Path.Combine(MakioClient.CoreModFolder, "engine.cfg");
        }

        public override void LoadConfig()
        {
            MakioClient.Log($"LoadConfig: Engine");


            if (!TryLoadConfig())
                SaveConfig();
        }

        public override void RegisterConfigElement<T>(ConfigElement<T> element)
        {
            
        }

        public override void SetConfigValue<T>(ConfigElement<T> element, T value)
        {
            // Not necessary
        }

        // Not necessary, just return the value.
        public override T GetConfigValue<T>(ConfigElement<T> element) => element.Value;

        // Always just auto-save.
        public override void OnAnyConfigChanged() => SaveConfig();

        public bool TryLoadConfig()
        {
            try
            {
                MakioClient.Log($"TryLoadConfig: Engine");


                if (!File.Exists(CONFIG_PATH))
                    return false;

                TomlDocument document = TomlParser.ParseFile(CONFIG_PATH);

                var config = TomletMain.To<EngineConfig>(File.ReadAllText(CONFIG_PATH));

                SlowMoTimeScale.Value = config.SlowMoTimeScale;
                Radius.Value = config.Radius;
                MinLaunchSpeed.Value = config.MinLaunchSpeed;
                MinCessSlideSpeed.Value = config.MinLaunchSpeed;
                StepSize.Value = config.StepSize;
                DragCoefficient.Value = config.DragCoefficient;
                MassDensity.Value = config.MassDensity;
                ReferenceArea.Value = config.ReferenceArea;
                Gravity.Value = config.Gravity;
                ErrorMargin.Value = config.ErrorMargin;
                DebugMode.Value = config.DebugMode;

                MaxJumpPreparationTime.Value = config.MaxJumpPreparationTime;
                MinJumpPowerFactor.Value = config.MinJumpPowerFactor;
                MaxJumpPower.Value = config.MaxJumpPower;
                JumpPower.Value = config.JumpPower;
                HighestAirPointRelativeTime.Value = config.HighestAirPointRelativeTime;                
                TrickSpinSpeed.Value = config.TrickSpinSpeed;
                MaxFlatSpinSpeed.Value = config.MaxFlatSpinSpeed;
                MaxCorkScrewSpeed.Value = config.MaxCorkScrewSpeed;
                MaxBioSpeed.Value = config.MaxBioSpeed;
                MaxFlipSpeed.Value = config.MaxFlipSpeed;
                TrickLandingTime.Value = config.TrickLandingTime;
                MinLongAirTime.Value = config.MinLongAirTime;



                return true;
            }
            catch (Exception ex)
            {
                MakioClient.LogWarning("Error loading internal data: " + ex.ToString());
                return false;
            }
        }

        public override void SaveConfig()
        {
            MakioClient.Log($"Saving Engine Config: {ConfigManager.EgnineConfigs.Count} EgnineConfigs");

            TomlDocument tomlDocument = TomlDocument.CreateEmpty();

            tomlDocument.Put(SlowMoTimeScale.Name, SlowMoTimeScale.Value, false);
            tomlDocument.Put(Radius.Name, Radius.Value, false);
            tomlDocument.Put(MinLaunchSpeed.Name, MinLaunchSpeed.Value, false);
            tomlDocument.Put(MinCessSlideSpeed.Name, MinCessSlideSpeed.Value, false);
            tomlDocument.Put(StepSize.Name, StepSize.Value, false);
            tomlDocument.Put(DragCoefficient.Name, DragCoefficient.Value, false);
            tomlDocument.Put(MassDensity.Name, MassDensity.Value, false);
            tomlDocument.Put(ReferenceArea.Name, ReferenceArea.Value, false);
            tomlDocument.Put(Gravity.Name, Gravity.Value, false);
            tomlDocument.Put(ErrorMargin.Name, ErrorMargin.Value, false);
            tomlDocument.Put(DebugMode.Name, DebugMode.Value, false);

            tomlDocument.Put(MaxJumpPreparationTime.Name, MaxJumpPreparationTime.Value, false);
            tomlDocument.Put(MinJumpPowerFactor.Name, MinJumpPowerFactor.Value, false);
            tomlDocument.Put(MaxJumpPower.Name, MaxJumpPower.Value, false);
            tomlDocument.Put(JumpPower.Name, JumpPower.Value, false);
            tomlDocument.Put(HighestAirPointRelativeTime.Name, HighestAirPointRelativeTime.Value, false);            
            tomlDocument.Put(TrickSpinSpeed.Name, TrickSpinSpeed.Value, false);
            tomlDocument.Put(MaxFlatSpinSpeed.Name, MaxFlatSpinSpeed.Value, false);
            tomlDocument.Put(MaxCorkScrewSpeed.Name, MaxCorkScrewSpeed.Value, false);
            tomlDocument.Put(MaxBioSpeed.Name, MaxBioSpeed.Value, false);
            tomlDocument.Put(MaxFlipSpeed.Name, MaxFlipSpeed.Value, false);
            tomlDocument.Put(TrickLandingTime.Name, TrickLandingTime.Value, false);
            tomlDocument.Put(MinLongAirTime.Name, MinLongAirTime.Value, false);


            if (Mod.ModPatches.Patch_BladeController.bladeController == null)
                return;

            var bladeController = Mod.ModPatches.Patch_BladeController.bladeController;

            //bladeController._globalsContainer.InAirProperties.maxJumpPreparationTime = MaxJumpPreparationTime.Value;
            //bladeController._globalsContainer.InAirProperties.minJumpPowerFactor = MinJumpPowerFactor.Value;
            //bladeController._globalsContainer.InAirProperties.maxJumpPower = MaxJumpPower.Value;
            bladeController._globalsContainer.InAirProperties.jumpPower = JumpPower.Value;
            bladeController._globalsContainer.InAirProperties.highestAirPointRelativeTime = HighestAirPointRelativeTime.Value;            
            bladeController._globalsContainer.InAirProperties.trickSpinSpeed = TrickSpinSpeed.Value;
            bladeController._globalsContainer.InAirProperties.maxFlatSpinSpeed = MaxFlatSpinSpeed.Value;
            bladeController._globalsContainer.InAirProperties.maxCorkScrewSpeed = MaxCorkScrewSpeed.Value;
            bladeController._globalsContainer.InAirProperties.maxBioSpeed = MaxBioSpeed.Value;
            bladeController._globalsContainer.InAirProperties.maxFlipSpeed = MaxFlipSpeed.Value;
            bladeController._globalsContainer.InAirProperties.trickLandingTime = TrickLandingTime.Value;
            //bladeController._globalsContainer.InAirProperties.minLongAirTime = MinLongAirTime.Value;





            File.WriteAllText(CONFIG_PATH, tomlDocument.SerializedValue);
        }


        


        public void CreateConfigElements()
        {

            SlowMoTimeScale = new("SlowMoTimeScale", "How much should the game speed slow to when in slowmo mode", .1f, ConfigType.Engine, ConfigBladeClass.General);
            
            MaxJumpPreparationTime = new("MaxJumpPreparationTime", "Undocumented (Requires Restart)", .5f, ConfigType.Engine, ConfigBladeClass.General);
            MinJumpPowerFactor = new("MinJumpPowerFactor", "Undocumented (Requires Restart)", .65f, ConfigType.Engine, ConfigBladeClass.General);
            MaxJumpPower = new("MaxJumpPower", "Undocumented (Requires Restart)", 4.5f, ConfigType.Engine, ConfigBladeClass.General);
            JumpPower = new("JumpPower", "Undocumented", 8f, ConfigType.Engine, ConfigBladeClass.General);
            HighestAirPointRelativeTime = new("HighestAirPointRelativeTime", "Undocumented", 1f, ConfigType.Engine, ConfigBladeClass.General);
            TrickSpinSpeed = new("TrickSpinSpeed", "Undocumented", 270, ConfigType.Engine, ConfigBladeClass.General);
            MaxFlatSpinSpeed = new("MaxFlatSpinSpeed", "Undocumented", 1080, ConfigType.Engine, ConfigBladeClass.General);
            MaxCorkScrewSpeed = new("MaxCorkScrewSpeed", "Undocumented", 1080, ConfigType.Engine, ConfigBladeClass.General);
            MaxBioSpeed = new("MaxBioSpeed", "Undocumented", 1080, ConfigType.Engine, ConfigBladeClass.General);
            MaxFlipSpeed = new("MaxFlipSpeed", "Undocumented", 1080, ConfigType.Engine, ConfigBladeClass.General);
            TrickLandingTime = new("TrickLandingTime", "Undocumented", .2f, ConfigType.Engine, ConfigBladeClass.General);
            MinLongAirTime = new("MinLongAirTime", "Undocumented (Requires Restart)", 1.5f, ConfigType.Engine, ConfigBladeClass.General);

            Radius = new("Radius", "Undocumented (Requires Restart)", .4f, ConfigType.Engine, ConfigBladeClass.General);
            MinLaunchSpeed = new("MinLaunchSpeed", "What is the minimum speed required to launch the player (Requires Restart)", 6f, ConfigType.Engine, ConfigBladeClass.General);
            MinCessSlideSpeed = new("MinCessSlideSpeed", "What is the minimum speed required for a CessSlide (Requires Restart)", 6f, ConfigType.Engine, ConfigBladeClass.General);
            StepSize = new("StepSize", "Undocumented (Requires Restart)", 6f, ConfigType.Engine, ConfigBladeClass.General);
            DragCoefficient = new("DragCoefficient", "Undocumented (Requires Restart)", 1.2f, ConfigType.Engine, ConfigBladeClass.General);
            MassDensity = new("MassDensity", "Undocumented (Requires Restart)", 1.2f, ConfigType.Engine, ConfigBladeClass.General);
            ReferenceArea = new("ReferenceArea", "Undocumented (Requires Restart)", 1f, ConfigType.Engine, ConfigBladeClass.General);
            Gravity = new("Gravity", "Take off gravity (Requires Restart)", 9.81f, ConfigType.Engine, ConfigBladeClass.General, "g");
            ErrorMargin = new("ErrorMargin", "Undocumented (Requires Restart)", 1f, ConfigType.Engine, ConfigBladeClass.General);
            DebugMode = new("DebugMode", "Enable Debugging Mode", false, ConfigType.Engine, ConfigBladeClass.General, "debug");



            DebugMode.OnValueChangedNotify = new Action(() =>
            {
                if (Mod.ModPatches.Patch_BladeGlobalsContainer.bladeGlobalsContainer == null)
                    return;

                var bladeGlobalsContainer = Mod.ModPatches.Patch_BladeGlobalsContainer.bladeGlobalsContainer;

                MakioClient.Log("DEBUG CHANGED");

                bladeGlobalsContainer.General.debug = DebugMode.Value;
            });

            //ConfigManager.EngineSaveData.Add(Radius.Name, Radius);
            //ConfigManager.EngineSaveData.Add(MinLaunchSpeed.Name, MinLaunchSpeed);
            //ConfigManager.EngineSaveData.Add(MinCessSlideSpeed.Name, MinCessSlideSpeed);
            //ConfigManager.EngineSaveData.Add(StepSize.Name, StepSize);
            //ConfigManager.EngineSaveData.Add(DragCoefficient.Name, DragCoefficient);
            //ConfigManager.EngineSaveData.Add(MassDensity.Name, MassDensity);
            //ConfigManager.EngineSaveData.Add(ReferenceArea.Name, ReferenceArea);
            //ConfigManager.EngineSaveData.Add(Gravity.Name, Gravity);
            //ConfigManager.EngineSaveData.Add(ErrorMargin.Name, ErrorMargin);
        }
    }
}
