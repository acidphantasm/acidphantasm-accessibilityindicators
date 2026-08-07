using AccessibilityIndicators.IndicatorUI;
using UnityEngine;

namespace AccessibilityIndicators.Scripts
{
    using Helpers;

    internal class KeepNorthRotation : MonoBehaviour
    {
        private bool isActuallyActive;

        public void Awake()
        {
            isActuallyActive = true;
        }

        public void Update()
        {
            if (isActuallyActive)
            {
                var player = Utility.GetMainPlayer();
                Transform camera = player.CameraPosition;
                float lookDirection = camera.transform.rotation.eulerAngles.y;

                Panel.HUDCenterPoint.transform.rotation = Quaternion.Euler(0, 0, lookDirection + Panel.NorthDirection);
            }
        }

        public void Stop()
        {
            Plugin.LogSource.LogInfo("[Accessibility Indicators] KeepNorthRotation Disabled");
            isActuallyActive = false;
        }
    }
}
