using AccessibilityIndicators.Helpers;
using AccessibilityIndicators.Scripts;
using UnityEngine;

namespace AccessibilityIndicators.IndicatorUI
{
    using System;

    internal class Panel : MonoBehaviour
    {
        public static GameObject IndicatorHUDPrefab;

        public static GameObject ShotPivotPrefab;
        public static GameObject StepPivotPrefab;
        public static GameObject VoicePivotPrefab;
        public static GameObject VerticalityPivotPrefab;

        public static GameObject IndicatorHUD;
        public static GameObject HUDCenterPoint;

        public static Vector3 NorthVector;
        public static float NorthDirection;

        public static int PoolObjectsShots;
        public static int PoolObjectsSteps;
        public static int PoolObjectsVoice;
        public static int PoolObjectsVerticality;

        private static KeepNorthRotation keepNorthRotationScript;

        public static void CreateHUD()
        {
            if (IndicatorHUD != null) return;

            try
            {
                IndicatorHUD = Instantiate(IndicatorHUDPrefab);
                HUDCenterPoint = IndicatorHUD.transform.GetChild(0).gameObject;
                ObjectPool.PoolShotIndicators(ShotPivotPrefab, HUDCenterPoint, PoolObjectsShots);
                ObjectPool.PoolStepIndicators(StepPivotPrefab, HUDCenterPoint, PoolObjectsSteps);
                ObjectPool.PoolVoiceIndicators(VoicePivotPrefab, HUDCenterPoint, PoolObjectsVoice);
                ObjectPool.PoolVerticalityIndicators(VerticalityPivotPrefab, HUDCenterPoint, PoolObjectsVerticality);
                IndicatorHUD.AddComponent<KeepNorthRotation>();
                keepNorthRotationScript = IndicatorHUD.GetOrAddComponent<KeepNorthRotation>();
                Plugin.LogSource.LogInfo("[Accessibility Indicators] Creating HUD");
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogInfo("[Accessibility Indicators] Failed to create HUD" + ex);
                throw;
            }
        }

        public static void Dispose()
        {
            Plugin.LogSource.LogInfo("[Accessibility Indicators] Cleaning up HUD");
            keepNorthRotationScript.Stop();
        }
    }
}
