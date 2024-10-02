using acidphantasm_accessibilityindicators.IndicatorUI;
using BepInEx.Configuration;
using System;
using UnityEngine;

namespace acidphantasm_accessibilityindicators
{
    internal static class AccessibilityIndicatorsConfig
    {
        private const string ConfigShotsTitle = "Shots Configuration";
        public static ConfigEntry<float> maxDistanceShots;
        public static ConfigEntry<float> fadeTimeShots;
        public static ConfigEntry<int> poolObjectsShots;
        private const string ConfigStepsTitle = "Steps Configuration";
        public static ConfigEntry<float> maxDistanceSteps;
        public static ConfigEntry<float> fadeTimeSteps;
        public static ConfigEntry<int> poolObjectsSteps;

        public static void InitConfig(ConfigFile config)
        {
            maxDistanceShots = config.Bind(ConfigShotsTitle, "Max Distance", 150f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 250f)));
            fadeTimeShots = config.Bind(ConfigShotsTitle, "Fade Time", 1f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.1f, 2f)));
            poolObjectsShots = config.Bind(ConfigShotsTitle, "Pool Objects", 50, new ConfigDescription("Number of indicator clones to make. Increase this if indicators are not showing when they should.", new AcceptableValueRange<int>(25, 100)));

            maxDistanceSteps = config.Bind(ConfigStepsTitle, "Max Distance", 50f, new ConfigDescription("Max distance from sound to show indicator.", new AcceptableValueRange<float>(1f, 75f)));
            fadeTimeSteps = config.Bind(ConfigStepsTitle, "Fade Time", 0.5f, new ConfigDescription("Amount of time in seconds for indicator to fade.", new AcceptableValueRange<float>(0.1f, 2f)));
            poolObjectsSteps = config.Bind(ConfigStepsTitle, "Pool Objects", 50, new ConfigDescription("Number of indicator clones to make. Increase this if indicators are not showing when they should.", new AcceptableValueRange<int>(25, 100)));

            Indicators.maxDistanceShots = maxDistanceShots.Value;
            Indicators.fadeTimeShots = fadeTimeShots.Value;
            Indicators.poolObjectsShots = poolObjectsShots.Value;

            Indicators.maxDistanceSteps = maxDistanceSteps.Value;
            Indicators.fadeTimeSteps = fadeTimeSteps.Value;
            Indicators.poolObjectsSteps = poolObjectsSteps.Value;

            maxDistanceShots.SettingChanged += Accessibility_SettingChanged;
            fadeTimeShots.SettingChanged += Accessibility_SettingChanged;
            poolObjectsShots.SettingChanged += Accessibility_SettingChanged;

            maxDistanceSteps.SettingChanged += Accessibility_SettingChanged;
            fadeTimeSteps.SettingChanged += Accessibility_SettingChanged;
            poolObjectsSteps.SettingChanged += Accessibility_SettingChanged;
        }

        private static void Accessibility_SettingChanged(object sender, EventArgs e)
        {
            Indicators.maxDistanceShots = maxDistanceShots.Value;
            Indicators.fadeTimeShots = fadeTimeShots.Value;
            Indicators.poolObjectsShots = poolObjectsShots.Value;

            Indicators.maxDistanceSteps = maxDistanceSteps.Value;
            Indicators.fadeTimeSteps = fadeTimeSteps.Value;
            Indicators.poolObjectsSteps = poolObjectsSteps.Value;
        }
    }
}