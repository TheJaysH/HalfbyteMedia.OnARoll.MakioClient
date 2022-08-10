global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using UnityEngine;
global using UnityEngine.UI;
global using UniverseLib;
global using UniverseLib.Utility;

using BepInEx;
using HalfbyteMedia.OnARoll.MakioClient.Config;
using HalfbyteMedia.OnARoll.MakioClient.Loader;
using HalfbyteMedia.OnARoll.MakioClient.UI;

namespace HalfbyteMedia.OnARoll.MakioClient
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class MakioClient
    {
        public const string GUID = "com.halfbytemedia.plugins.onaroll.makio";
        public const string NAME = "Makio Client";
        public const string VERSION = "1.0.0.0";
        public const string DEFAULT_EXPLORER_FOLDER_NAME = "makio-client";

        public static ILoader Loader { get; private set; }
        public static string CoreModFolder => Path.Combine(Loader.CoreModFolderDestination, Loader.CoreModFolderName);

        public static HarmonyLib.Harmony Harmony { get; } = new HarmonyLib.Harmony(GUID);


        public static void Init(ILoader loader)
        {
            if (Loader != null)
                throw new Exception($"{NAME} is already loaded.");

            Loader = loader;

            Log($"{NAME} {VERSION} initializing...");

            Directory.CreateDirectory(CoreModFolder);
            ConfigManager.Init(Loader.ConfigHandler);            
  
            Universe.Init(1f, LateInit, Log, new()
            {
                Disable_EventSystem_Override = false,
                Force_Unlock_Mouse = true,
                Unhollowed_Modules_Folder = loader.UnhollowedModulesFolder
            });

            Runtime.RuntimeHelper.Init();
            Behaviour.Setup();

            Harmony.PatchAll();
            //UnityCrashPrevention.Init();
        }


        static void LateInit()
        {
            Log($"Setting up late core features...");

            //SceneHandler.Init();

            Log($"Creating UI...");

            UIManager.InitUI();

            Log($"{NAME} {VERSION} ({Universe.Context}) initialized.");
        }

        internal static void Update()
        {
            // check master toggle
            if (Input.GetKeyDown(ConfigManager.Master_Toggle.Value))
                UIManager.ShowMenu = !UIManager.ShowMenu;

            UIManager.Update();
            Mod.ModLogic.Update();
            //QuickStart();
        }

        private static void QuickStart()
        {
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (currentScene.buildIndex == 0)
            {
                //LoadingScreen.LoadLevel(Levels.spl);

                //var sceneIndex = 1;
                //SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                //var startMenuScene = SceneManager.GetSceneByBuildIndex(sceneIndex);

                //if (startMenuScene.isLoaded)
                //    SceneManager.SetActiveScene(startMenuScene);
            }
        }

        #region LOGGING

        public static void Log(object message)
            => Log(message, LogType.Log);

        public static void LogWarning(object message)
            => Log(message, LogType.Warning);

        public static void LogError(object message)
            => Log(message, LogType.Error);

        public static void LogUnity(object message, LogType logType)
        {
            //if (!ConfigManager.Log_Unity_Debug.Value)
            //    return;

            //Log($"[Unity] {message}", logType);
        }

        private static void Log(object message, LogType logType)
        {
            string log = message?.ToString() ?? "";

            //LogPanel.Log(log, logType);

            switch (logType)
            {
                case LogType.Assert:
                case LogType.Log:
                    Loader.OnLogMessage(log);
                    break;

                case LogType.Warning:
                    Loader.OnLogWarning(log);
                    break;

                case LogType.Error:
                case LogType.Exception:
                    Loader.OnLogError(log);
                    break;
            }
        }

        #endregion
    }



}

