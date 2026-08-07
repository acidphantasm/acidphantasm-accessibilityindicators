using BepInEx.Logging;
using BepInEx;
using System;
using AccessibilityIndicators.Patches;
using AccessibilityIndicators.IndicatorUI;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace AccessibilityIndicators
{
    [BepInPlugin("com.acidphantasm.accessibilityindicators", "acidphantasm-AccessibilityIndicators", "2.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        public static Plugin Instance;

        internal void Awake()
        {
            LogSource = Logger;
            if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
            {
                throw new Exception($"Invalid EFT Version");
            }

            Instance = this;
            DontDestroyOnLoad(this);

            AccessibilityIndicatorsConfig.InitAAConfig(Config);

            new GameWorldOnGameStartedPatch().Enable();
            new GameWorldUnregisterPlayerPatch().Enable();
            new LevelSettingsPatch().Enable();
            new FirearmControllerPatch().Enable();
            new PhraseSpeakerClassPatch().Enable();
            new DefaultPlayPatch().Enable();
            new PlayStepSoundPatch().Enable();
            new PlayGearSoundPatch().Enable();
            
            LoadBundle();
        }

        private static void LoadBundle()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assetBundle = Path.Combine(directory, "assets", "accessibilityindicators.bundle");
            var bundle = AssetBundle.LoadFromFile(assetBundle);
            
            if (bundle == null)
            {
                throw new Exception($"Error loading bundles");
            }
            Panel.IndicatorHUDPrefab = LoadAsset<GameObject>(bundle, "Canvas.prefab");
            Panel.ShotPivotPrefab = LoadAsset<GameObject>(bundle, "shotPivot.prefab");
            Panel.StepPivotPrefab = LoadAsset<GameObject>(bundle, "runPivot.prefab");
            Panel.VoicePivotPrefab = LoadAsset<GameObject>(bundle, "voicePivot.prefab");
            Panel.VerticalityPivotPrefab = LoadAsset<GameObject>(bundle, "verticalityPivot.prefab");

            if (Panel.IndicatorHUDPrefab == null)
            {
                LogSource.LogInfo("Indicator HUD Prefab is null");
            }
        }
        
        private static T LoadAsset<T>(AssetBundle bundle, string assetPath) where T : UnityEngine.Object
        {
            T asset = bundle.LoadAsset<T>(assetPath);

            if (asset == null)
            {
                throw new Exception($"Error loading asset {assetPath}");
            }

            DontDestroyOnLoad(asset);
            return asset;
        }
    }
}
