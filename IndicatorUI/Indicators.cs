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
        public static float maxShotDistance;
        public static float fadeTimeShots;

        public static float maxSprintDistance;
        public static float fadeTimeSprint;

        public static float maxRunDistance;
        public static float fadeTimeRun;

        public static float maxSneakDistance;
        public static float fadeTimeSneak;

        public static bool enable;
        public static bool showTeammates;
        public static bool enableShots;
        public static bool enableRunSteps;
        public static bool enableSprintSteps;
        public static bool enableSneakSteps;

        public static bool normalizeDistance;
        public static float minNormalizedDistance;
        public static float maxNormalizedDistance;

        public static void PrepareShot(Vector3 shotPosition, string id, bool isTeammate)
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

            if (shotDistance <= maxShotDistance) DrawShotIndicator(realShotAngle, shotDistance, id, isTeammate);
        }
        public static void PrepareStep(EAudioMovementState movementState, Vector3 position, float distance, string id, bool isTeammate)
        {
            var player = Utils.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;
            float maxDistance;

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    maxDistance = maxSprintDistance;
                    break;
                case EAudioMovementState.Run:
                    maxDistance = maxRunDistance;
                    break;
                case EAudioMovementState.Duck:
                    maxDistance = maxSneakDistance;
                    break;
                default:
                    return;
            }

            var stepDirection = position - cameraPosition;

            var stepAngle = Utils.GetAngle(stepDirection);
            var playerAngle = Utils.GetLookAngle(camera.eulerAngles);

            var realStepAngle = stepAngle + playerAngle;

            if (realStepAngle > 360) realStepAngle = realStepAngle - 360;

            if (distance <= maxDistance) DrawStepIndicator(realStepAngle, distance, movementState, id, isTeammate);
        }
        private static void DrawShotIndicator(float shotAngle, float shotDistance, string accountID, bool isTeammate)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledShotObject(accountID);
            GameObject enemyShotIndicator = pivotIndicator.transform.GetChild(0).gameObject;
            GameObject teamShotIndicator = pivotIndicator.transform.GetChild(1).gameObject;
            GameObject selectedShotIndicator;

            if (isTeammate) selectedShotIndicator = teamShotIndicator;
            else selectedShotIndicator = enemyShotIndicator;

            Image image = selectedShotIndicator.GetComponent<Image>();

            float newMinDistance = normalizeDistance ? minNormalizedDistance : 1f;
            float newMaxDistance = normalizeDistance ? maxNormalizedDistance : maxShotDistance;

            float size = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 3.5f, 0.1f, shotDistance);
            float alpha = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0f, shotDistance);
            selectedShotIndicator.transform.localScale = new Vector3(size, size, 0);
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
            selectedShotIndicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeShots);
        }
        private static void DrawStepIndicator(float stepAngle, float stepDistance, EAudioMovementState movementState, string accountID, bool isTeammate)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledRunObject(accountID);
            GameObject enemySprintIndicator = pivotIndicator.transform.GetChild(0).gameObject;
            GameObject enemySneakIndicator = pivotIndicator.transform.GetChild(2).gameObject;
            GameObject enemyRunIndicator = pivotIndicator.transform.GetChild(4).gameObject;
            GameObject teamSprintIndicator = pivotIndicator.transform.GetChild(1).gameObject;
            GameObject teamSneakIndicator = pivotIndicator.transform.GetChild(3).gameObject;
            GameObject teamRunIndicator = pivotIndicator.transform.GetChild(5).gameObject;
            GameObject selectedStepIndicator;
            Image image;
            float fadeTime;

            float newMinDistance = normalizeDistance ? minNormalizedDistance : 1f;
            float newMaxDistance;

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    if (!enableSprintSteps) return;
                    if (isTeammate) selectedStepIndicator = teamSprintIndicator;
                    else selectedStepIndicator = enemySprintIndicator;

                    teamSneakIndicator.SetActive(false);
                    teamRunIndicator.SetActive(false);
                    enemySneakIndicator.SetActive(false);
                    enemyRunIndicator.SetActive(false);
                    fadeTime = fadeTimeSprint;
                    newMaxDistance = normalizeDistance ? maxNormalizedDistance : maxSprintDistance;

                    break;
                case EAudioMovementState.Run:
                    if (!enableRunSteps) return;
                    if (isTeammate) selectedStepIndicator = teamRunIndicator;
                    else selectedStepIndicator = enemyRunIndicator;

                    teamSneakIndicator.SetActive(false);
                    teamSprintIndicator.SetActive(false);
                    enemySneakIndicator.SetActive(false);
                    enemySprintIndicator.SetActive(false);
                    fadeTime = fadeTimeRun;
                    newMaxDistance = normalizeDistance ? maxNormalizedDistance : maxRunDistance;

                    break;
                case EAudioMovementState.Duck:
                    if (!enableSneakSteps) return;
                    if (isTeammate) selectedStepIndicator = teamSneakIndicator;
                    else selectedStepIndicator = enemySneakIndicator;

                    teamSprintIndicator.SetActive(false);
                    teamRunIndicator.SetActive(false);
                    enemySprintIndicator.SetActive(false);
                    enemyRunIndicator.SetActive(false);
                    fadeTime = fadeTimeSneak;
                    newMaxDistance = normalizeDistance ? maxNormalizedDistance : maxSneakDistance;

                    break;
                default:
                    return;
            }

            image = selectedStepIndicator.GetComponent<Image>();

            float size = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 3.5f, 0.1f, stepDistance);
            float alpha = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0f, stepDistance);
            selectedStepIndicator.transform.localScale = new Vector3(size, size, 0);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            if (pivotIndicator.activeInHierarchy)
            {
                selectedStepIndicator.SetActive(true);

                ObjectIDInfo objectInfo = pivotIndicator.GetComponent<ObjectIDInfo>();
                TempCoroutineRunner runner = pivotIndicator.GetComponent<TempCoroutineRunner>();
                runner.StopCoroutine(objectInfo._Coroutine);
                TempCoroutine.DisableAfterRefade(pivotIndicator, image, runner, fadeTime);
                pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
                return;
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
            pivotIndicator.SetActive(true);
            selectedStepIndicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTime);
        }
    }
}
