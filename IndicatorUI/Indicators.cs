using UnityEngine;
using acidphantasm_accessibilityindicators.Helpers;
using static acidphantasm_accessibilityindicators.Helpers.DebugGizmos;
using Audio.Data;
using Image = UnityEngine.UI.Image;
using static acidphantasm_accessibilityindicators.Helpers.DebugGizmos.TempCoroutine;


namespace acidphantasm_accessibilityindicators.IndicatorUI
{
    internal class Indicators : MonoBehaviour
    {
        private static float originalStepY = 160f;
        private static float originalShotY = 170f;
        private static float originalVerticalityY = 180f;
        private static float originalVoiceY = 188f;

        private static float scaleVoiceMin = 0.1f;
        private static float scaleVoiceMax = 0.6f;
        private static float scaleStepShotMin = 0.5f;
        private static float scaleStepShotMax = 1.5f;

        public static bool enableShots;
        public static float maxShotDistance;
        public static float fadeTimeShots;
        public static Color enemyShotColour;
        public static Color friendShotColour;
        public static float indicatorOffset;

        public static bool enableSprintSteps;
        public static float maxSprintDistance;
        public static float fadeTimeSprint;
        public static Color enemySprintColour;
        public static Color friendSprintColour;

        public static bool enableRunSteps;
        public static float maxRunDistance;
        public static float fadeTimeRun;
        public static Color enemyRunColour;
        public static Color friendRunColour;

        public static bool enableSneakSteps;
        public static float maxSneakDistance;
        public static float fadeTimeSneak;
        public static Color enemySneakColour;
        public static Color friendSneakColour;

        public static bool enableVoicelines;
        public static float maxVoiceDistance;
        public static float fadeTimeVoice;

        public static bool enable;
        public static bool showTeammates;

        public static bool normalizeDistance;
        public static float minNormalizedDistance;
        public static float maxNormalizedDistance;

        public static void PrepareVoice(Vector3 voicePosition, string id, bool isTeammate)
        {
            var player = Utils.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;

            float voiceDistance = Utils.GetDistance(cameraPosition, voicePosition);
            Vector3 voiceDirection = voicePosition - cameraPosition;

            float voiceAngle = Utils.GetAngle(voiceDirection);
            float playerAngle = Utils.GetLookAngle(camera.eulerAngles);

            var realVoiceAngle = voiceAngle + playerAngle;

            if (realVoiceAngle > 360) realVoiceAngle = realVoiceAngle - 360;

            if (voiceDistance <= maxVoiceDistance) DrawVoiceIndicator(realVoiceAngle, voiceDistance, id, isTeammate);
        }
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

            VerticalityValues value = Utils.AboveOrBelowCheck(cameraPosition.y, shotPosition.y);

            if (realShotAngle > 360) realShotAngle = realShotAngle - 360;

            if (shotDistance <= maxShotDistance) DrawShotIndicator(realShotAngle, shotDistance, value, id, isTeammate);
        }
        public static void PrepareStep(EAudioMovementState movementState, Vector3 stepPosition, float distance, string id, bool isTeammate)
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

            var stepDirection = stepPosition - cameraPosition;

            var stepAngle = Utils.GetAngle(stepDirection);
            var playerAngle = Utils.GetLookAngle(camera.eulerAngles);

            var realStepAngle = stepAngle + playerAngle;

            VerticalityValues value = Utils.AboveOrBelowCheck(cameraPosition.y, stepPosition.y);

            if (realStepAngle > 360) realStepAngle = realStepAngle - 360;

            if (distance <= maxDistance) DrawStepIndicator(realStepAngle, distance, value, movementState, id, isTeammate);
        }
        private static void DrawVoiceIndicator(float voiceAngle, float voiceDistance, string accountID, bool isTeammate)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledVoiceObject(accountID);
            GameObject pivotArm = pivotIndicator.transform.GetChild(0).gameObject;
            GameObject voiceIndicator = pivotArm.transform.GetChild(0).gameObject;

            Image image = voiceIndicator.GetComponent<Image>();

            float newMinDistance = normalizeDistance ? minNormalizedDistance : 1f;
            float newMaxDistance = normalizeDistance ? maxNormalizedDistance : maxVoiceDistance;

            float size = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, scaleVoiceMax, scaleVoiceMin, voiceDistance);
            float alpha = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0.1f, voiceDistance);
            voiceIndicator.transform.localScale = new Vector3(size, size, 0);
            pivotArm.transform.localPosition = new Vector3(pivotArm.transform.localPosition.x, originalVoiceY + indicatorOffset, pivotArm.transform.localPosition.z);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            if (pivotIndicator.activeInHierarchy)
            {
                ObjectIDInfo objectInfo = pivotIndicator.GetComponent<ObjectIDInfo>();
                TempCoroutineRunner runner = pivotIndicator.GetComponent<TempCoroutineRunner>();
                runner.StopCoroutine(objectInfo._Coroutine);
                TempCoroutine.DisableAfterRefade(pivotIndicator, image, runner, fadeTimeVoice);
                pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, voiceAngle);
                return;
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, voiceAngle);
            pivotIndicator.SetActive(true);
            pivotArm.SetActive(true);
            voiceIndicator.SetActive(true);

            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeVoice);
        }
        private static void DrawShotIndicator(float shotAngle, float shotDistance, VerticalityValues value, string accountID, bool isTeammate)
        {
            GameObject verticalityPivotIndicator = ObjectPool.GetPooledVerticalityObject(accountID);
            GameObject belowArmPivotIndicator = verticalityPivotIndicator.transform.GetChild(0).gameObject;
            GameObject aboveArmPivotIndicator = verticalityPivotIndicator.transform.GetChild(1).gameObject;
            GameObject belowIndicator = belowArmPivotIndicator.transform.GetChild(0).gameObject;
            GameObject aboveIndicator = aboveArmPivotIndicator.transform.GetChild(0).gameObject;
            GameObject selectedVerticalityArmPivot;
            GameObject selectedVerticalityIndicator;
            Image vertImage;

            GameObject pivotIndicator = ObjectPool.GetPooledShotObject(accountID);
            GameObject enemyShotIndicator = pivotIndicator.transform.GetChild(0).gameObject;
            GameObject teamShotIndicator = pivotIndicator.transform.GetChild(1).gameObject;
            GameObject selectedShotIndicator;
            Image image;

            if (isTeammate) selectedShotIndicator = teamShotIndicator;
            else selectedShotIndicator = enemyShotIndicator;

            switch (value)
            {
                case VerticalityValues.Above:
                    selectedVerticalityArmPivot = aboveArmPivotIndicator;
                    selectedVerticalityIndicator = aboveIndicator;
                    vertImage = selectedVerticalityIndicator.GetComponentInChildren<Image>();
                    belowArmPivotIndicator.SetActive(false);
                    break;
                case VerticalityValues.Below:
                    selectedVerticalityArmPivot = belowArmPivotIndicator;
                    selectedVerticalityIndicator = belowIndicator;
                    vertImage = selectedVerticalityIndicator.GetComponentInChildren<Image>();
                    aboveArmPivotIndicator.SetActive(false);
                    break;
                default:
                    selectedVerticalityArmPivot = null;
                    selectedVerticalityIndicator = null;
                    vertImage = null;
                    belowArmPivotIndicator.SetActive(false);
                    aboveArmPivotIndicator.SetActive(false);
                    break;
            }
            image = selectedShotIndicator.GetComponent<Image>();

            float newMinDistance = normalizeDistance ? minNormalizedDistance : 1f;
            float newMaxDistance = normalizeDistance ? maxNormalizedDistance : maxShotDistance;

            float size = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, scaleStepShotMax, scaleStepShotMin, shotDistance);
            float alpha = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0.1f, shotDistance);
            selectedShotIndicator.transform.localScale = new Vector3(size, size, 0);
            selectedShotIndicator.transform.localPosition = new Vector3(selectedShotIndicator.transform.localPosition.x, originalShotY + indicatorOffset, selectedShotIndicator.transform.localPosition.z);

            if (isTeammate) image.color = new Color(friendShotColour.r, friendShotColour.g, friendShotColour.b, alpha);
            else image.color = new Color(enemyShotColour.r, enemyShotColour.g, enemyShotColour.b, alpha);


            if (pivotIndicator.activeInHierarchy)
            {
                if (verticalityPivotIndicator.activeInHierarchy && selectedVerticalityIndicator != null)
                {
                    selectedVerticalityIndicator.SetActive(true);
                    selectedVerticalityArmPivot.SetActive(true);
                    ObjectIDInfo vertObjectInfo = verticalityPivotIndicator.GetComponent<ObjectIDInfo>();
                    TempCoroutineRunner vertRunner = verticalityPivotIndicator.GetComponent<TempCoroutineRunner>();
                    vertRunner.StopCoroutine(vertObjectInfo._Coroutine);
                    TempCoroutine.DisableAfterRefade(verticalityPivotIndicator, vertImage, vertRunner, fadeTimeShots);
                    verticalityPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
                }
                ObjectIDInfo objectInfo = pivotIndicator.GetComponent<ObjectIDInfo>();
                TempCoroutineRunner runner = pivotIndicator.GetComponent<TempCoroutineRunner>();
                runner.StopCoroutine(objectInfo._Coroutine);
                TempCoroutine.DisableAfterRefade(pivotIndicator, image, runner, fadeTimeShots);
                pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
                
                return;
            }

            if (selectedVerticalityIndicator != null)
            {
                verticalityPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
                verticalityPivotIndicator.SetActive(true);
                selectedVerticalityArmPivot.SetActive(true);
                selectedVerticalityIndicator.SetActive(true);
                TempCoroutine.DisableAfterFade(verticalityPivotIndicator, vertImage, fadeTimeShots);
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
            pivotIndicator.SetActive(true);
            selectedShotIndicator.SetActive(true);
            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTimeShots);
        }
        private static void DrawStepIndicator(float stepAngle, float stepDistance, VerticalityValues value, EAudioMovementState movementState, string accountID, bool isTeammate)
        {
            GameObject verticalityPivotIndicator = ObjectPool.GetPooledVerticalityObject(accountID);
            GameObject belowArmPivotIndicator = verticalityPivotIndicator.transform.GetChild(0).gameObject;
            GameObject aboveArmPivotIndicator = verticalityPivotIndicator.transform.GetChild(1).gameObject;
            GameObject belowIndicator = belowArmPivotIndicator.transform.GetChild(0).gameObject;
            GameObject aboveIndicator = aboveArmPivotIndicator.transform.GetChild(0).gameObject;
            GameObject selectedVerticalityArmPivot;
            GameObject selectedVerticalityIndicator;
            Image vertImage;

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

            switch (value)
            {
                case VerticalityValues.Above:
                    selectedVerticalityArmPivot = aboveArmPivotIndicator;
                    selectedVerticalityIndicator = aboveIndicator;
                    vertImage = selectedVerticalityIndicator.GetComponentInChildren<Image>();
                    belowArmPivotIndicator.SetActive(false);
                    break;
                case VerticalityValues.Below:
                    selectedVerticalityArmPivot = belowArmPivotIndicator;
                    selectedVerticalityIndicator = belowIndicator;
                    vertImage = selectedVerticalityIndicator.GetComponentInChildren<Image>();
                    aboveArmPivotIndicator.SetActive(false);
                    break;
                default:
                    selectedVerticalityArmPivot = null;
                    selectedVerticalityIndicator = null;
                    vertImage = null;
                    belowArmPivotIndicator.SetActive(false);
                    aboveArmPivotIndicator.SetActive(false);
                    break;
            }

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
            float size = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, scaleStepShotMax, scaleStepShotMin, stepDistance);
            float alpha = Utils.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0.1f, stepDistance);
            selectedStepIndicator.transform.localScale = new Vector3(size, size, 0);
            selectedStepIndicator.transform.localPosition = new Vector3(selectedStepIndicator.transform.localPosition.x, originalStepY + indicatorOffset, selectedStepIndicator.transform.localPosition.z);

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    if (isTeammate) image.color = new Color(friendSprintColour.r, friendSprintColour.g, friendSprintColour.b, alpha);
                    else image.color = new Color(enemySprintColour.r, enemySprintColour.g, enemySprintColour.b, alpha);
                    break;
                case EAudioMovementState.Duck:
                    if (isTeammate) image.color = new Color(friendSneakColour.r, friendSneakColour.g, friendSneakColour.b, alpha);
                    else image.color = new Color(enemySneakColour.r, enemySneakColour.g, enemySneakColour.b, alpha);
                    break;
                case EAudioMovementState.Run:
                    if (isTeammate) image.color = new Color(friendRunColour.r, friendRunColour.g, friendRunColour.b, alpha);
                    else image.color = new Color(enemyRunColour.r, enemyRunColour.g, enemyRunColour.b, alpha);
                    break;
                default:
                    return;
            }

            if (pivotIndicator.activeInHierarchy)
            {
                if (verticalityPivotIndicator.activeInHierarchy && selectedVerticalityIndicator != null)
                {
                    selectedVerticalityArmPivot.transform.localPosition = new Vector3(selectedVerticalityArmPivot.transform.localPosition.x, originalVerticalityY + indicatorOffset, selectedVerticalityArmPivot.transform.localPosition.z);
                    selectedVerticalityIndicator.SetActive(true);
                    selectedVerticalityArmPivot.SetActive(true);
                    ObjectIDInfo vertObjectInfo = verticalityPivotIndicator.GetComponent<ObjectIDInfo>();
                    TempCoroutineRunner vertRunner = verticalityPivotIndicator.GetComponent<TempCoroutineRunner>();
                    vertRunner.StopCoroutine(vertObjectInfo._Coroutine);
                    TempCoroutine.DisableAfterRefade(verticalityPivotIndicator, vertImage, vertRunner, fadeTime);
                    verticalityPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
                }
                selectedStepIndicator.SetActive(true);
                ObjectIDInfo objectInfo = pivotIndicator.GetComponent<ObjectIDInfo>();
                TempCoroutineRunner runner = pivotIndicator.GetComponent<TempCoroutineRunner>();
                runner.StopCoroutine(objectInfo._Coroutine);
                TempCoroutine.DisableAfterRefade(pivotIndicator, image, runner, fadeTime);
                pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
                return;
            }

            if (selectedVerticalityIndicator != null)
            {
                selectedVerticalityArmPivot.transform.localPosition = new Vector3(selectedVerticalityArmPivot.transform.localPosition.x, originalVerticalityY + indicatorOffset, selectedVerticalityArmPivot.transform.localPosition.z);
                verticalityPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
                verticalityPivotIndicator.SetActive(true);
                selectedVerticalityArmPivot.SetActive(true);
                selectedVerticalityIndicator.SetActive(true);
                TempCoroutine.DisableAfterFade(verticalityPivotIndicator, vertImage, fadeTime);
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
            pivotIndicator.SetActive(true);
            selectedStepIndicator.SetActive(true);
            TempCoroutine.DisableAfterFade(pivotIndicator, image, fadeTime);
        }
    }
}
