using acidphantasm_accessibilityindicators.IndicatorUI;
using BepInEx.Configuration;
using System;
using UnityEngine;

namespace acidphantasm_accessibilityindicators
{
    internal static class AccessibilityIndicatorsConfig
    {
        private static int loadOrder = 100;
        private const string TogglesTitle = "Mod Toggles";
        public static ConfigEntry<bool> enable;
        public static ConfigEntry<bool> showTeammates;
        public static ConfigEntry<bool> enableRealTimeIndicators;

        private const string ConfigShotsTitle = "Shots Configuration";
        public static ConfigEntry<bool> enableShots;
        public static ConfigEntry<float> maxDistanceShots;
        public static ConfigEntry<float> fadeTimeShots;
        public static ConfigEntry<int> poolObjectsShots;

        private const string ConfigStepsTitle = "Steps Configuration";
        public static ConfigEntry<bool> enableRunSteps;
        public static ConfigEntry<bool> enableSprintSteps;
        public static ConfigEntry<float> maxDistanceSteps;
        public static ConfigEntry<float> fadeTimeSteps;
        public static ConfigEntry<int> poolObjectsSteps;

        public static void InitAAConfig(ConfigFile config)
        {
            // Toggles
            enable = config.Bind(TogglesTitle, "Enable Mod", true, new ConfigDescription("Enable or disable this mod.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            showTeammates = config.Bind(TogglesTitle, "Show Teammates", false, new ConfigDescription("If you enjoy Swedish Coffee, this toggle is for you. Toggle showing indicators for teammates.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            enableRealTimeIndicators = config.Bind(TogglesTitle, "Test Indicators", false, new ConfigDescription("Ties the indicators to specific bot instances, updates indicator location on next sound.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));

            Indicators.enable = enable.Value;
            Indicators.showTeammates = showTeammates.Value;
            Indicators.enableRealTimeIndicators = enableRealTimeIndicators.Value;


            // Shots
            enableShots = config.Bind(ConfigShotsTitle, "Enable Shots", true, new ConfigDescription("Enable or disable shot indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxDistanceShots = config.Bind(ConfigShotsTitle, "Max Distance", 150f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 300f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            fadeTimeShots = config.Bind(ConfigShotsTitle, "Fade Time", 1f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            poolObjectsShots = config.Bind(ConfigShotsTitle, "Pool Objects", 50, new ConfigDescription("Number of indicator clones to make. Increase this if indicators are not showing when they should.", new AcceptableValueRange<int>(25, 100), new ConfigurationManagerAttributes { Order = loadOrder-- }));

            Indicators.enableShots = enableShots.Value;
            Indicators.maxDistanceShots = maxDistanceShots.Value;
            Indicators.fadeTimeShots = fadeTimeShots.Value;
            Indicators.poolObjectsShots = poolObjectsShots.Value;


            // Steps
            enableRunSteps = config.Bind(ConfigStepsTitle, "Enable Walk Steps", true, new ConfigDescription("Enable or disable walk step indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            enableSprintSteps = config.Bind(ConfigStepsTitle, "Enable Sprint Steps", true, new ConfigDescription("Enable or disable sprint step indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxDistanceSteps = config.Bind(ConfigStepsTitle, "Max Distance", 50f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 75f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            fadeTimeSteps = config.Bind(ConfigStepsTitle, "Fade Time", 0.75f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            poolObjectsSteps = config.Bind(ConfigStepsTitle, "Pool Objects", 100, new ConfigDescription("Number of indicator clones to make. Increase this if indicators are not showing when they should.", new AcceptableValueRange<int>(50, 150), new ConfigurationManagerAttributes { Order = loadOrder-- }));

            Indicators.enableRunSteps = enableRunSteps.Value;
            Indicators.enableSprintSteps = enableSprintSteps.Value;
            Indicators.maxDistanceSteps = maxDistanceSteps.Value;
            Indicators.fadeTimeSteps = fadeTimeSteps.Value;
            Indicators.poolObjectsSteps = poolObjectsSteps.Value;


            // Triggers
            enable.SettingChanged += Accessibility_SettingChanged;
            showTeammates.SettingChanged += Accessibility_SettingChanged;
            enableRealTimeIndicators.SettingChanged += Accessibility_SettingChanged;

            enableShots.SettingChanged += Accessibility_SettingChanged;
            maxDistanceShots.SettingChanged += Accessibility_SettingChanged;
            fadeTimeShots.SettingChanged += Accessibility_SettingChanged;
            poolObjectsShots.SettingChanged += Accessibility_SettingChanged;

            enableRunSteps.SettingChanged += Accessibility_SettingChanged;
            enableSprintSteps.SettingChanged += Accessibility_SettingChanged;
            maxDistanceSteps.SettingChanged += Accessibility_SettingChanged;
            fadeTimeSteps.SettingChanged += Accessibility_SettingChanged;
            poolObjectsSteps.SettingChanged += Accessibility_SettingChanged;

        }

        private static void Accessibility_SettingChanged(object sender, EventArgs e)
        {
            Indicators.enable = enable.Value;
            Indicators.showTeammates = showTeammates.Value;
            Indicators.enableRealTimeIndicators = enableRealTimeIndicators.Value;

            Indicators.enableShots = enableShots.Value;
            Indicators.maxDistanceShots = maxDistanceShots.Value;
            Indicators.fadeTimeShots = fadeTimeShots.Value;
            Indicators.poolObjectsShots = poolObjectsShots.Value;

            Indicators.enableRunSteps = enableRunSteps.Value;
            Indicators.enableSprintSteps = enableSprintSteps.Value;
            Indicators.maxDistanceSteps = maxDistanceSteps.Value;
            Indicators.fadeTimeSteps = fadeTimeSteps.Value;
            Indicators.poolObjectsSteps = poolObjectsSteps.Value;
        }
    }
}