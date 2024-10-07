using acidphantasm_accessibilityindicators.Helpers;
using EFT.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static RootMotion.FinalIK.GenericPoser;

namespace acidphantasm_accessibilityindicators.IndicatorUI
{
    internal class Panel : MonoBehaviour
    {
        public static GameObject IndicatorHUDPrefab;
        public static GameObject HUDCenterPoint;
        public static GameObject ShotPivotPrefab;
        public static GameObject RunPivotPrefab;
        public static GameObject SprintPivotPrefab;
        public static GameObject IndicatorHUD;

        public static Vector3 northVector;
        public static float northDirection;

        public static int poolObjectsShots;
        public static int poolObjectsSteps;

        public static void CreateHUD()
        {
            if (IndicatorHUD != null) return;

            IndicatorHUD = Instantiate(IndicatorHUDPrefab);
            HUDCenterPoint = IndicatorHUD.transform.GetChild(0).gameObject;
            ObjectPool.PoolShotIndicators(ShotPivotPrefab, HUDCenterPoint, poolObjectsShots);
            ObjectPool.PoolRunIndicators(RunPivotPrefab, HUDCenterPoint, poolObjectsSteps);
            IndicatorHUD.AddComponent<Panel>();

        }

        public void Update()
        {
            var player = Utils.GetMainPlayer();
            Transform camera = player.CameraPosition;
            float lookDirection = camera.transform.rotation.eulerAngles.y;

            HUDCenterPoint.transform.rotation = Quaternion.Euler(0, 0, lookDirection + northDirection);
        }
    }
}
