using BepInEx.Logging;
using BepInEx;
using System;
using acidphantasm_accessibilityindicators.Patches;
using acidphantasm_accessibilityindicators.IndicatorUI;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace acidphantasm_accessibilityindicators
{
    [BepInPlugin("phantasm.acid.accessibilityindicators", "acidphantasm-AccessibilityIndicators", "1.1.0")]
    [BepInDependency("com.SPT.core", "3.9.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        public static Plugin Instance;

        internal void Awake()
        {
            LogSource = Logger;

            Instance = this;
            DontDestroyOnLoad(this);

            LogSource.LogInfo("[AccessibilityIndicators] loading...");

            AccessibilityIndicatorsConfig.InitConfig(Config);

            new FirearmControllerPatch().Enable();
            new LevelSettingsNorthVectorPatch().Enable();
            new LevelSettingsNorthDirectionPatch().Enable();
            new PlayerDefaultPlayPatch().Enable();
            new PlayerPlayStepSoundPatch().Enable();
            new PlayerMethod50Patch().Enable();

            LogSource.LogInfo("[AccessibilityIndicators] loaded!");
        }

        private void Start()
        {
            LoadBundle();
        }

        public static void LoadBundle()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assetBundle = Path.Combine(directory, "Assets", "accessibility.bundle");
            var bundle = AssetBundle.LoadFromFile(assetBundle);
            if (bundle == null)
            {
                throw new Exception($"Error loading bundles");
            }
            Indicators.IndicatorHUDPrefab = LoadAsset<GameObject>(bundle, "Canvas.prefab");
            Indicators.ShotPivotPrefab = LoadAsset<GameObject>(bundle, "shotPivot.prefab");
            Indicators.RunPivotPrefab = LoadAsset<GameObject>(bundle, "runPivot.prefab");
            Indicators.SprintPivotPrefab = LoadAsset<GameObject>(bundle, "sprintPivot.prefab");
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
