using UnityEngine;
using AccessibilityIndicators.Helpers;
using Image = UnityEngine.UI.Image;
using CommonAssets.Scripts.Audio;


namespace AccessibilityIndicators.IndicatorUI
{
    internal class Indicators : MonoBehaviour
    {
        private static float originalStepY = 160f;
        private static float originalShotY = 170f;
        private static float originalVerticalityY = 180f;
        private static float originalVoiceY = 188f;

        private static float scaleVoiceMin = 0.05f;
        private static float scaleVoiceMax = 0.2f;
        private static float scaleShotMin = 0.25f;
        private static float scaleShotMax = 1.25f;
        private static float scaleStepMin = 0.05f;
        private static float scaleStepMax = 1.25f;

        public static bool EnableShots;
        public static float MaxShotDistance;
        public static float FadeTimeShots;
        public static Color EnemyShotColour;
        public static Color FriendShotColour;
        public static float IndicatorOffset;

        public static bool EnableSprintSteps;
        public static float MaxSprintDistance;
        public static float FadeTimeSprint;
        public static Color EnemySprintColour;
        public static Color FriendSprintColour;

        public static bool EnableWalkSteps;
        public static float MaxWalkDistance;
        public static float FadeTimeWalk;
        public static Color EnemyWalkColour;
        public static Color FriendWalkColour;

        public static bool EnableSneakSteps;
        public static float MaxSneakDistance;
        public static float FadeTimeSneak;
        public static Color EnemySneakColour;
        public static Color FriendSneakColour;

        public static bool EnableVoicelines;
        public static float MaxVoiceDistance;
        public static float FadeTimeVoice;

        public static bool Enable;
        public static bool ShowTeammates;

        public static bool NormalizeDistance;
        public static float MinNormalizedDistance;
        public static float MaxNormalizedDistance;

        public static void PrepareVoice(Vector3 voicePosition, string id, bool isTeammate)
        {
            var player = Utility.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;

            float voiceDistance = Utility.GetDistance(cameraPosition, voicePosition);
            if (voiceDistance > MaxVoiceDistance) return;

            Vector3 voiceDirection = voicePosition - cameraPosition;

            float voiceAngle = Utility.GetAngle(voiceDirection);
            float playerAngle = Utility.GetLookAngle(camera.eulerAngles);

            var realVoiceAngle = voiceAngle + playerAngle;

            if (realVoiceAngle > 360) realVoiceAngle = realVoiceAngle - 360;

            if (voiceDistance <= MaxVoiceDistance) DrawVoiceIndicator(realVoiceAngle, voiceDistance, id, isTeammate);
        }
        public static void PrepareShot(Vector3 shotPosition, string id, bool isTeammate)
        {
            var player = Utility.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;

            var shotDistance = Utility.GetDistance(cameraPosition, shotPosition);
            if (shotDistance > MaxShotDistance) return; 

            var shotDirection = shotPosition - cameraPosition;

            var shotAngle = Utility.GetAngle(shotDirection);
            var playerAngle = Utility.GetLookAngle(camera.eulerAngles);
            var realShotAngle = shotAngle + playerAngle;

            VerticalityValues value = Utility.AboveOrBelowCheck(cameraPosition.y, shotPosition.y);
            if (realShotAngle > 360) realShotAngle = realShotAngle - 360;
            DrawShotIndicator(realShotAngle, shotDistance, value, id, isTeammate);
        }
        public static void PrepareStep(EAudioMovementState movementState, Vector3 stepPosition, float distance, string id, bool isTeammate)
        {
            var player = Utility.GetMainPlayer();
            Transform camera = player.CameraPosition;
            Vector3 cameraPosition = camera.position;
            float maxDistance;

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    if (!EnableSprintSteps) return;
                    maxDistance = MaxSprintDistance;
                    break;
                case EAudioMovementState.Run:
                    if (!EnableWalkSteps) return;
                    maxDistance = MaxWalkDistance;
                    break;
                case EAudioMovementState.Duck:
                    if (!EnableSneakSteps) return;
                    maxDistance = MaxSneakDistance;
                    break;
                default:
                    return;
            }
            if (distance > maxDistance) return;

            var stepDirection = stepPosition - cameraPosition;

            var stepAngle = Utility.GetAngle(stepDirection);
            var playerAngle = Utility.GetLookAngle(camera.eulerAngles);

            var realStepAngle = stepAngle + playerAngle;

            VerticalityValues value = Utility.AboveOrBelowCheck(cameraPosition.y, stepPosition.y);

            if (realStepAngle > 360) realStepAngle = realStepAngle - 360;

            DrawStepIndicator(realStepAngle, distance, value, movementState, id, isTeammate);
        }
        private static void DrawVoiceIndicator(float voiceAngle, float voiceDistance, string accountID, bool isTeammate)
        {
            GameObject pivotIndicator = ObjectPool.GetPooledVoiceObject(accountID);
            GameObject pivotArm = pivotIndicator.transform.GetChild(0).gameObject;
            GameObject voiceIndicator = pivotArm.transform.GetChild(0).gameObject;

            Image image = voiceIndicator.GetComponent<Image>();

            float newMinDistance = NormalizeDistance ? MinNormalizedDistance : 1f;
            float newMaxDistance = NormalizeDistance ? MaxNormalizedDistance : MaxVoiceDistance;

            float size = Utility.CustomInverseLerp(newMinDistance, newMaxDistance, scaleVoiceMax, scaleVoiceMin, voiceDistance);
            float alpha = Utility.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0.1f, voiceDistance);
            voiceIndicator.transform.localScale = new Vector3(size, size, 0);
            pivotArm.transform.localPosition = new Vector3(pivotArm.transform.localPosition.x, originalVoiceY + IndicatorOffset, pivotArm.transform.localPosition.z);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, voiceAngle);
            pivotIndicator.SetActive(true);
            pivotArm.SetActive(true);
            voiceIndicator.SetActive(true);

            pivotIndicator.GetOrAddComponent<CoroutineHandler>().StartRestartFade(pivotIndicator, image, FadeTimeVoice);
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

            GameObject shotPivotIndicator = ObjectPool.GetPooledShotObject(accountID);
            GameObject shotIndicator = shotPivotIndicator.transform.GetChild(0).gameObject;
            Image image = shotIndicator.GetComponent<Image>();

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

            float newMinDistance = NormalizeDistance ? MinNormalizedDistance : 1f;
            float newMaxDistance = NormalizeDistance ? MaxNormalizedDistance : MaxShotDistance;

            float size = Utility.CustomInverseLerp(newMinDistance, newMaxDistance, scaleShotMax, scaleShotMin, shotDistance);
            float alpha = Utility.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0.1f, shotDistance);
            shotIndicator.transform.localScale = new Vector3(size, size, 0);
            shotIndicator.transform.localPosition = new Vector3(shotIndicator.transform.localPosition.x, originalShotY + IndicatorOffset, shotIndicator.transform.localPosition.z);

            if (isTeammate) image.color = new Color(FriendShotColour.r, FriendShotColour.g, FriendShotColour.b, alpha);
            else image.color = new Color(EnemyShotColour.r, EnemyShotColour.g, EnemyShotColour.b, alpha);


            if (selectedVerticalityIndicator != null)
            {
                verticalityPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
                verticalityPivotIndicator.SetActive(true);
                selectedVerticalityArmPivot.SetActive(true);
                selectedVerticalityIndicator.SetActive(true);
                verticalityPivotIndicator.GetOrAddComponent<CoroutineHandler>().StartRestartFade(verticalityPivotIndicator, vertImage, FadeTimeShots);
            }

            shotPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, shotAngle);
            shotPivotIndicator.SetActive(true);
            shotIndicator.SetActive(true);
            shotPivotIndicator.GetOrAddComponent<CoroutineHandler>().StartRestartFade(shotPivotIndicator, image, FadeTimeShots);
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
            Image verticalityImage;

            GameObject pivotIndicator = ObjectPool.GetPooledStepObject(accountID);
            GameObject selectedStepIndicator = pivotIndicator.transform.GetChild(0).gameObject;
            Image image = selectedStepIndicator.GetComponent<Image>();

            float fadeTime;
            float newMinDistance = NormalizeDistance ? MinNormalizedDistance : 1f;
            float newMaxDistance;

            switch (value)
            {
                case VerticalityValues.Above:
                    selectedVerticalityArmPivot = aboveArmPivotIndicator;
                    selectedVerticalityIndicator = aboveIndicator;
                    verticalityImage = selectedVerticalityIndicator.GetComponentInChildren<Image>();
                    belowArmPivotIndicator.SetActive(false);
                    break;
                case VerticalityValues.Below:
                    selectedVerticalityArmPivot = belowArmPivotIndicator;
                    selectedVerticalityIndicator = belowIndicator;
                    verticalityImage = selectedVerticalityIndicator.GetComponentInChildren<Image>();
                    aboveArmPivotIndicator.SetActive(false);
                    break;
                default:
                    selectedVerticalityArmPivot = null;
                    selectedVerticalityIndicator = null;
                    verticalityImage = null;
                    belowArmPivotIndicator.SetActive(false);
                    aboveArmPivotIndicator.SetActive(false);
                    break;
            }

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    fadeTime = FadeTimeSprint;
                    newMaxDistance = NormalizeDistance ? MaxNormalizedDistance : MaxSprintDistance;
                    break;
                case EAudioMovementState.Run:
                    fadeTime = FadeTimeWalk;
                    newMaxDistance = NormalizeDistance ? MaxNormalizedDistance : MaxWalkDistance;
                    break;
                case EAudioMovementState.Duck:
                    fadeTime = FadeTimeSneak;
                    newMaxDistance = NormalizeDistance ? MaxNormalizedDistance : MaxSneakDistance;
                    break;
                default:
                    return;
            }

            float size = Utility.CustomInverseLerp(newMinDistance, newMaxDistance, scaleStepMax, scaleStepMin, stepDistance);
            float alpha = Utility.CustomInverseLerp(newMinDistance, newMaxDistance, 1f, 0.1f, stepDistance);
            selectedStepIndicator.transform.localScale = new Vector3(size, size, 0);
            selectedStepIndicator.transform.localPosition = new Vector3(selectedStepIndicator.transform.localPosition.x, originalStepY + IndicatorOffset, selectedStepIndicator.transform.localPosition.z);

            switch (movementState)
            {
                case EAudioMovementState.Sprint:
                    if (isTeammate) image.color = new Color(FriendSprintColour.r, FriendSprintColour.g, FriendSprintColour.b, alpha);
                    else image.color = new Color(EnemySprintColour.r, EnemySprintColour.g, EnemySprintColour.b, alpha);
                    break;
                case EAudioMovementState.Duck:
                    if (isTeammate) image.color = new Color(FriendSneakColour.r, FriendSneakColour.g, FriendSneakColour.b, alpha);
                    else image.color = new Color(EnemySneakColour.r, EnemySneakColour.g, EnemySneakColour.b, alpha);
                    break;
                case EAudioMovementState.Run:
                    if (isTeammate) image.color = new Color(FriendWalkColour.r, FriendWalkColour.g, FriendWalkColour.b, alpha);
                    else image.color = new Color(EnemyWalkColour.r, EnemyWalkColour.g, EnemyWalkColour.b, alpha);
                    break;
                default:
                    return;
            }

            if (selectedVerticalityIndicator != null)
            {
                selectedVerticalityArmPivot.transform.localPosition = new Vector3(selectedVerticalityArmPivot.transform.localPosition.x, originalVerticalityY + IndicatorOffset, selectedVerticalityArmPivot.transform.localPosition.z);
                verticalityPivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
                verticalityPivotIndicator.SetActive(true);
                selectedVerticalityArmPivot.SetActive(true);
                selectedVerticalityIndicator.SetActive(true);
                verticalityPivotIndicator.GetOrAddComponent<CoroutineHandler>().StartRestartFade(verticalityPivotIndicator, verticalityImage, fadeTime);
            }

            pivotIndicator.transform.rotation = Quaternion.Euler(0, 0, stepAngle);
            pivotIndicator.SetActive(true);
            selectedStepIndicator.SetActive(true);
            pivotIndicator.GetOrAddComponent<CoroutineHandler>().StartRestartFade(pivotIndicator, image, fadeTime);
        }
    }
}
