using Comfort.Common;
using EFT;
using UnityEngine;
using acidphantasm_accessibilityindicators.Patches;
using acidphantasm_accessibilityindicators.Helpers;
using static acidphantasm_accessibilityindicators.Helpers.DebugGizmos;
using Audio.Data;
using Image = UnityEngine.UI.Image;


namespace acidphantasm_accessibilityindicators.IndicatorUI
{

    internal class Indicators : MonoBehaviour
    {
        public static GameObject IndicatorHUDPrefab;
        public static GameObject ShotPivotPrefab;
        public static GameObject RunPivotPrefab;
        public static GameObject SprintPivotPrefab;
        public static GameObject IndicatorHUD;
        public static float maxDistanceShots;
        public static float fadeTimeShots;
        public static float maxDistanceSteps;
        public static float fadeTimeSteps;

        public static int poolObjectsShots;
        public static int poolObjectsSteps;

        public static bool enable;
        public static bool showTeammates;
        public static bool enableShots;
        public static bool enableRunSteps;
        public static bool enableSprintSteps;

        public static void BuildHUD()
        {
            if (IndicatorHUD == null)
            {
                IndicatorHUD = Instantiate(IndicatorHUDPrefab);
                GameObject centerPoint = IndicatorHUD.transform.GetChild(0).gameObject;
                ObjectPool.PoolShotIndicators(ShotPivotPrefab, centerPoint, poolObjectsShots);
                ObjectPool.PoolRunIndicators(RunPivotPrefab, centerPoint, poolObjectsSteps);
                ObjectPool.PoolSprintIndicators(SprintPivotPrefab, centerPoint, poolObjectsSteps);
            }
        }
        public static void DisposeHUD()
        {
            IndicatorHUD.SetActive(false);
        }
        public static void PrepareShot(Vector3 shotPosition, string id)
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            Transform camera = gameWorld.MainPlayer.CameraPosition;
            Vector3 cameraPosition = camera.position;

            var shotDistance = GetDistance(cameraPosition, shotPosition);
            var shotDirection = shotPosition - cameraPosition;

            var shotAngle = GetAngle(shotDirection);
            var playerAngle = GetLookAngle(camera.eulerAngles);

            var realShotAngle = shotAngle + playerAngle;

            if (realShotAngle > 360) realShotAngle = realShotAngle - 360;

            if (shotDistance <= maxDistanceShots) DrawShotIndicator(realShotAngle, shotDistance, id);

            //Plugin.LogSource.LogInfo($"PlayerLookDir: {playerAngle} | ShotAngle: {realShotAngle} | Distance: {shotDistance}m");
        }
        public static void PrepareStep(EAudioMovementState movementState, Vector3 position, float distance, string id)
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            Transform camera = gameWorld.MainPlayer.CameraPosition;
            Vector3 cameraPosition = camera.position;

            var stepDirection = position - cameraPosition;

            var stepAngle = GetAngle(stepDirection);
            var playerAngle = GetLookAngle(camera.eulerAngles);

            var realStepAngle = stepAngle + playerAngle;

            if (realStepAngle > 360) realStepAngle = realStepAngle - 360;

            if (distance <= maxDistanceSteps) DrawStepIndicator(realStepAngle, distance, movementState, id);

            //Plugin.LogSource.LogInfo($"PlayerLookDir: {playerAngle} | StepAngle: {realStepAngle} | Distance: {distance}m | MovementState: {movementState}");
        }
        private static void DrawShotIndicator(float shotAngle, float shotDistance, string id)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledShotObject(id);
            GameObject indicator = pivotIndicator.transform.GetChild(0).gameObject;
            Image image = indicator.GetComponent<Image>();

            float size = CustomInverseLerp(1, maxDistanceShots, 3.5f, 1f, shotDistance);
            float alpha = CustomInverseLerp(1, maxDistanceShots, 1f, 0f, shotDistance);

            indicator.transform.localScale = new Vector3(size, size, 0);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);


            pivotIndicator.transform.Rotate(0, 0, shotAngle);
            pivotIndicator.SetActive(true);
            indicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeShots);
        }
        private static void DrawStepIndicator(float stepAngle, float stepDistance, EAudioMovementState movementState, string id)
        {
            if (IndicatorHUD == null)
            {
                IndicatorHUD = Instantiate(IndicatorHUDPrefab);
                GameObject centerPoint = IndicatorHUD.transform.GetChild(0).gameObject;
                ObjectPool.PoolShotIndicators(ShotPivotPrefab, centerPoint, poolObjectsShots);
                ObjectPool.PoolRunIndicators(RunPivotPrefab, centerPoint, poolObjectsSteps);
                ObjectPool.PoolSprintIndicators(SprintPivotPrefab, centerPoint, poolObjectsSteps);
            }

            GameObject pivotIndicator;
            GameObject indicator;
            Image image;

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    if (!enableSprintSteps) return;
                    pivotIndicator = ObjectPool.GetPooledSprintObject(id);
                    break;
                case EAudioMovementState.Run:
                    if (!enableRunSteps) return;
                    pivotIndicator = ObjectPool.GetPooledRunObject(id);
                    break;
                default:
                    return;
            }

            indicator = pivotIndicator.transform.GetChild(0).gameObject;
            image = indicator.GetComponent<Image>();

            float size = CustomInverseLerp(1, maxDistanceSteps, 3.5f, 1f, stepDistance);
            float alpha = CustomInverseLerp(1, maxDistanceSteps, 1f, 0f, stepDistance);
            indicator.transform.localScale = new Vector3(size, size, 0);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            pivotIndicator.transform.Rotate(0, 0, stepAngle);
            pivotIndicator.SetActive(true);
            indicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeSteps);
        }
        private static float GetDistance(Vector3 from, Vector3 to)
        {
            float distance = Vector3.Distance(from, to);
            return distance;
        }
        private static float GetAngle(Vector3 originDirection)
        {
            var levelSettings = Singleton<LevelSettings>.Instance;
            originDirection.y = 0;
            var angle = Vector3.SignedAngle(originDirection, levelSettings.NorthVector, Vector3.up);
            if (angle >= 0) return angle;
            return angle + 360;
        }
        private static float GetLookAngle(Vector3 originAngle)
        {
            var levelSettings = Singleton<LevelSettings>.Instance;
            var angle = originAngle.y - levelSettings.NorthDirection;

            if (angle >= 0) return angle;
            return angle + 360;
        }

        private static float CustomInverseLerp(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }
    }
}
