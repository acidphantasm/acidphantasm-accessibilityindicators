using acidphantasm_accessibilityindicators.IndicatorUI;
using BepInEx.Configuration;
using System;
using UnityEngine;

namespace acidphantasm_accessibilityindicators
{
    internal static class AccessibilityIndicatorsConfig
    {
        private static int loadOrder = 100;
        private const string ConfigGeneral = "1. General";
        public static ConfigEntry<bool> enable;
        public static ConfigEntry<bool> showTeammates;
        public static ConfigEntry<bool> normalizeDistance;
        public static ConfigEntry<float> maxNormalizedDistance;
        public static ConfigEntry<float> minNormalizedDistance;

        private const string ConfigShotsTitle = "2. Shots Configuration";
        public static ConfigEntry<bool> enableShots;
        public static ConfigEntry<float> maxShotDistance;
        public static ConfigEntry<float> fadeTimeShots;

        private const string ConfigStepsTitle = "3. Footsteps Configuration";
        public static ConfigEntry<bool> enableSprintSteps;
        public static ConfigEntry<float> maxSprintDistance;
        public static ConfigEntry<float> fadeTimeSprint;

        public static ConfigEntry<bool> enableRunSteps;
        public static ConfigEntry<float> maxRunDistance;
        public static ConfigEntry<float> fadeTimeRun;

        public static ConfigEntry<bool> enableSneakSteps;
        public static ConfigEntry<float> maxSneakDistance;
        public static ConfigEntry<float> fadeTimeSneak;

        private const string ConfigAdvancedTitle = "4. Advanced";
        public static ConfigEntry<int> poolObjectsShots;
        public static ConfigEntry<int> poolObjectsSteps;


        public static void InitAAConfig(ConfigFile config)
        {
            // General Settings
            enable = config.Bind(ConfigGeneral, "Enable Mod", true, new ConfigDescription("Enable or disable this mod.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            showTeammates = config.Bind(ConfigGeneral, "Show Teammates", false, new ConfigDescription("If you enjoy Swedish Coffee, this toggle is for you. Toggle showing indicators for teammates.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            normalizeDistance = config.Bind(ConfigGeneral, "Normalize Indicator Size", true, new ConfigDescription("Toggles normalizing the size of the indicator based on distance. Disabled means each indicator type will size respective to it's configured max distance, enabled means all indicator types will be the same size at the same distance.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            minNormalizedDistance = config.Bind(ConfigGeneral, "Min Normalized Distance", 1f, new ConfigDescription("Indicator will be it's largest when the sound made is this close (or closer) to you.", new AcceptableValueRange<float>(0.1f, 24f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxNormalizedDistance = config.Bind(ConfigGeneral, "Max Normalized Distance", 75f, new ConfigDescription("Indicator will be it's smallest when the sound made is this far (or further) from you.", new AcceptableValueRange<float>(25f, 300f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            Indicators.enable = enable.Value;
            Indicators.showTeammates = showTeammates.Value;
            Indicators.normalizeDistance = normalizeDistance.Value;
            Indicators.minNormalizedDistance = minNormalizedDistance.Value;
            Indicators.maxNormalizedDistance = maxNormalizedDistance.Value;

            // Shots
            enableShots = config.Bind(ConfigShotsTitle, "Enable Shots", true, new ConfigDescription("Enable or disable shot indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxShotDistance = config.Bind(ConfigShotsTitle, "Max Distance", 150f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 300f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            fadeTimeShots = config.Bind(ConfigShotsTitle, "Fade Time", 1f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            Indicators.enableShots = enableShots.Value;
            Indicators.maxShotDistance = maxShotDistance.Value;
            Indicators.fadeTimeShots = fadeTimeShots.Value;


            // Steps
            enableSprintSteps = config.Bind(ConfigStepsTitle, "Enable Sprint Steps", true, new ConfigDescription("Enable or disable sprint step indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxSprintDistance = config.Bind(ConfigStepsTitle, "Max Sprint Distance", 50f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 75f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            fadeTimeSprint = config.Bind(ConfigStepsTitle, "Sprint Fade Time", 0.75f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            Indicators.enableSprintSteps = enableSprintSteps.Value;
            Indicators.maxSprintDistance = maxSprintDistance.Value;
            Indicators.fadeTimeSprint = fadeTimeSprint.Value;

            enableRunSteps = config.Bind(ConfigStepsTitle, "Enable Walk Steps", true, new ConfigDescription("Enable or disable walk step indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxRunDistance = config.Bind(ConfigStepsTitle, "Max Walk Distance", 30f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            fadeTimeRun = config.Bind(ConfigStepsTitle, "Run Fade Time", 0.75f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            Indicators.enableRunSteps = enableRunSteps.Value;
            Indicators.maxRunDistance = maxRunDistance.Value;
            Indicators.fadeTimeRun = fadeTimeRun.Value;

            enableSneakSteps = config.Bind(ConfigStepsTitle, "Enable Sneak Steps", true, new ConfigDescription("Enable or disable sneak step indicators.", null, new ConfigurationManagerAttributes { Order = loadOrder-- }));
            maxSneakDistance = config.Bind(ConfigStepsTitle, "Max Sneak Distance", 15f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 25f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            fadeTimeSneak = config.Bind(ConfigStepsTitle, "Sneak Fade Time", 0.75f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { Order = loadOrder-- }));
            Indicators.enableSneakSteps = enableSneakSteps.Value;
            Indicators.maxSneakDistance = maxSneakDistance.Value;
            Indicators.fadeTimeSneak = fadeTimeSneak.Value;

            // Advanced
            poolObjectsSteps = config.Bind(ConfigAdvancedTitle, "Shots Pool Objects", 75, new ConfigDescription("Number of indicator clones to make. Increase this if indicators are not showing when they should.", new AcceptableValueRange<int>(50, 150), new ConfigurationManagerAttributes { IsAdvanced = true, Order = loadOrder-- }));
            poolObjectsShots = config.Bind(ConfigAdvancedTitle, "Footstep Pool Objects", 75, new ConfigDescription("Number of indicator clones to make. Increase this if indicators are not showing when they should.", new AcceptableValueRange<int>(50, 150), new ConfigurationManagerAttributes { IsAdvanced = true, Order = loadOrder-- }));
            Panel.poolObjectsSteps = poolObjectsSteps.Value;
            Panel.poolObjectsShots = poolObjectsShots.Value;

            // Triggers
            enable.SettingChanged += Accessibility_SettingChanged;
            showTeammates.SettingChanged += Accessibility_SettingChanged;
            normalizeDistance.SettingChanged += Accessibility_SettingChanged;
            minNormalizedDistance.SettingChanged += Accessibility_SettingChanged;
            maxNormalizedDistance.SettingChanged += Accessibility_SettingChanged;

            enableShots.SettingChanged += Accessibility_SettingChanged;
            maxShotDistance.SettingChanged += Accessibility_SettingChanged;
            fadeTimeShots.SettingChanged += Accessibility_SettingChanged;
            poolObjectsShots.SettingChanged += Accessibility_SettingChanged;

            enableSprintSteps.SettingChanged += Accessibility_SettingChanged;
            maxSprintDistance.SettingChanged += Accessibility_SettingChanged;
            fadeTimeSprint.SettingChanged += Accessibility_SettingChanged;

            enableRunSteps.SettingChanged += Accessibility_SettingChanged;
            maxRunDistance.SettingChanged += Accessibility_SettingChanged;
            fadeTimeRun.SettingChanged += Accessibility_SettingChanged;

            enableSneakSteps.SettingChanged += Accessibility_SettingChanged;
            maxSneakDistance.SettingChanged += Accessibility_SettingChanged;
            fadeTimeSneak.SettingChanged += Accessibility_SettingChanged;

            poolObjectsSteps.SettingChanged += Accessibility_SettingChanged;
        }

        private static void Accessibility_SettingChanged(object sender, EventArgs e)
        {
            Indicators.enable = enable.Value;
            Indicators.showTeammates = showTeammates.Value;
            Indicators.normalizeDistance = normalizeDistance.Value;
            Indicators.minNormalizedDistance = minNormalizedDistance.Value;
            Indicators.maxNormalizedDistance = maxNormalizedDistance.Value;

            Indicators.enableShots = enableShots.Value;
            Indicators.maxShotDistance = maxShotDistance.Value;
            Indicators.fadeTimeShots = fadeTimeShots.Value;

            Panel.poolObjectsShots = poolObjectsShots.Value;

            Indicators.enableSprintSteps = enableSprintSteps.Value;
            Indicators.maxSprintDistance = maxSprintDistance.Value;
            Indicators.fadeTimeSprint = fadeTimeSprint.Value;

            Indicators.enableRunSteps = enableRunSteps.Value;
            Indicators.maxRunDistance = maxRunDistance.Value;
            Indicators.fadeTimeRun = fadeTimeRun.Value;

            Indicators.enableSneakSteps = enableSneakSteps.Value;
            Indicators.maxSneakDistance = maxSneakDistance.Value;
            Indicators.fadeTimeSneak = fadeTimeSneak.Value;

            Panel.poolObjectsSteps = poolObjectsSteps.Value;
        }
    }
}