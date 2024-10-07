using Comfort.Common;
using EFT;
using UnityEngine;
using acidphantasm_accessibilityindicators.Patches;
using acidphantasm_accessibilityindicators.Helpers;
using static acidphantasm_accessibilityindicators.Helpers.DebugGizmos;
using Audio.Data;
using Image = UnityEngine.UI.Image;
using static acidphantasm_accessibilityindicators.Helpers.DebugGizmos.TempCoroutine;


namespace acidphantasm_accessibilityindicators.IndicatorUI
{

    internal class Indicators : MonoBehaviour
    {
        public static float maxDistanceShots;
        public static float fadeTimeShots;
        public static float maxDistanceSteps;
        public static float fadeTimeSteps;

        public static bool enable;
        public static bool showTeammates;
        public static bool enableShots;
        public static bool enableRunSteps;
        public static bool enableSprintSteps;

        public static void PrepareShot(Vector3 shotPosition, string id)
        {
            var player = Utils.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;

            var shotDistance = Utils.GetDistance(cameraPosition, shotPosition);
            var shotDirection = shotPosition - cameraPosition;

            var shotAngle = Utils.GetAngle(shotDirection);
            var playerAngle = Utils.GetLookAngle(camera.eulerAngles);

            var realShotAngle = shotAngle + playerAngle;

            if (realShotAngle > 360) realShotAngle = realShotAngle - 360;

            if (shotDistance <= maxDistanceShots) DrawShotIndicator(realShotAngle, shotDistance, id);
        }
        public static void PrepareStep(EAudioMovementState movementState, Vector3 position, float distance, string id)
        {
            var player = Utils.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;

            var stepDirection = position - cameraPosition;

            var stepAngle = Utils.GetAngle(stepDirection);
            var playerAngle = Utils.GetLookAngle(camera.eulerAngles);

            var realStepAngle = stepAngle + playerAngle;

            if (realStepAngle > 360) realStepAngle = realStepAngle - 360;

            if (distance <= maxDistanceSteps) DrawStepIndicator(realStepAngle, distance, movementState, id);
        }
        private static void DrawShotIndicator(float shotAngle, float shotDistance, string accountID)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledShotObject(accountID);
            GameObject indicator = pivotIndicator.transform.GetChild(0).gameObject;
            Image image = indicator.GetComponent<Image>();

            float size = Utils.CustomInverseLerp(1, maxDistanceShots, 3.5f, 1f, shotDistance);
            float alpha = Utils.CustomInverseLerp(1, maxDistanceShots, 1f, 0f, shotDistance);
            indicator.transform.localScale = new Vector3(size, size, 0);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            if (pivotIndicator.activeInHierarchy)
            {
                ObjectIDInfo objectInfo = pivotIndicator.GetComponent<ObjectIDInfo>();
                TempCoroutineRunner runner = pivotIndicator.GetComponent<TempCoroutineRunner>();
                runner.StopCoroutine(objectInfo._Coroutine);
                TempCoroutine.DisableAfterRefade(pivotIndicator, image, runner, fadeTimeShots);
                pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
                return;
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
            pivotIndicator.SetActive(true);
            indicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeShots);
        }
        private static void DrawStepIndicator(float stepAngle, float stepDistance, EAudioMovementState movementState, string accountID)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledRunObject(accountID);
            GameObject runIndicator = pivotIndicator.transform.GetChild(0).gameObject;
            GameObject sprintIndicator = pivotIndicator.transform.GetChild(1).gameObject;
            GameObject selectedIndicator;
            bool isSprinting;
            Image image;

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    if (!enableSprintSteps) return;
                    selectedIndicator = sprintIndicator;
                    runIndicator.SetActive(false);
                    isSprinting = true;
                    break;
                case EAudioMovementState.Run:
                    if (!enableRunSteps) return;
                    selectedIndicator = runIndicator;
                    sprintIndicator.SetActive(false);
                    isSprinting = false;
                    break;
                default:
                    return;
            }

            image = selectedIndicator.GetComponent<Image>();

            float size = Utils.CustomInverseLerp(1, maxDistanceSteps, 3.5f, 1f, stepDistance);
            float alpha = Utils.CustomInverseLerp(1, maxDistanceSteps, 1f, 0f, stepDistance);
            selectedIndicator.transform.localScale = new Vector3(size, size, 0);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            if (pivotIndicator.activeInHierarchy)
            {
                if (isSprinting) sprintIndicator.SetActive(true);
                else runIndicator.SetActive(true);

                ObjectIDInfo objectInfo = pivotIndicator.GetComponent<ObjectIDInfo>();
                TempCoroutineRunner runner = pivotIndicator.GetComponent<TempCoroutineRunner>();
                runner.StopCoroutine(objectInfo._Coroutine);
                TempCoroutine.DisableAfterRefade(pivotIndicator, image, runner, fadeTimeSteps);
                pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
                return;
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
            pivotIndicator.SetActive(true);
            selectedIndicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeSteps);
        }
    }
}
