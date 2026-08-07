using AccessibilityIndicators.Helpers;
using AccessibilityIndicators.IndicatorUI;
using Audio.Data;
using CommonAssets.Scripts.Audio;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace AccessibilityIndicators.Patches
{
    internal class DefaultPlayPatch : ModulePatch
    {
        private static FieldInfo distanceInfo;
        protected override MethodBase GetTargetMethod()
        {
            distanceInfo = AccessTools.Field(typeof(Player), "_distance");
            return AccessTools.Method(typeof(Player), nameof(Player.DefaultPlay));
        }

        [PatchPostfix]
        static void PatchPostfix(Player __instance, EAudioMovementState movementState)
        {
            if (__instance == null 
                || __instance.IsYourPlayer 
                || !Indicators.Enable
                || (!__instance.IsAI && __instance.IsGroupedWithMainPlayer() && !Indicators.ShowTeammates)) return;

            var position = __instance.Position;
            var distance = (float)distanceInfo.GetValue(__instance);
            var isTeammate = __instance.IsGroupedWithMainPlayer();

            Indicators.PrepareStep(movementState, position, distance, __instance.ProfileId, isTeammate);
        }
    }
    internal class PlayStepSoundPatch : ModulePatch
    {
        private static FieldInfo distanceInfo;
        protected override MethodBase GetTargetMethod()
        {
            distanceInfo = AccessTools.Field(typeof(Player), "_distance");
            return AccessTools.Method(typeof(Player), nameof(Player.PlayStepSound));
        }

        [PatchPostfix]
        static void PatchPostfix(Player __instance)
        {
            if (__instance == null
                || __instance.IsYourPlayer
                || !Indicators.Enable
                || (!__instance.IsAI && __instance.IsGroupedWithMainPlayer() && !Indicators.ShowTeammates)) return;

            var position = __instance.Position;
            var distance = (float)distanceInfo.GetValue(__instance);
            var eaudioMovementState = ((__instance.Pose == EPlayerPose.Duck) ? EAudioMovementState.Duck : EAudioMovementState.Run);
            var isTeammate = __instance.IsGroupedWithMainPlayer();

            Indicators.PrepareStep(eaudioMovementState, position, distance, __instance.ProfileId, isTeammate);
        }
    }
    internal class PlayGearSoundPatch : ModulePatch
    {
        private static FieldInfo distanceInfo;
        protected override MethodBase GetTargetMethod()
        {
            distanceInfo = AccessTools.Field(typeof(Player), "_distance");

            return AccessTools.Method(
            typeof(Player),
            nameof(Player.PlayGearSound),
            new[] { typeof(SoundBank), typeof(float) }
            );
        }

        [PatchPostfix]
        static void PatchPostfix(Player __instance)
        {
            if (__instance == null || __instance.IsYourPlayer || !Indicators.Enable || (!__instance.IsAI && __instance.IsGroupedWithMainPlayer() && !Indicators.ShowTeammates)) 
                return;

            if (__instance.CurrentState.Name is EPlayerState.Sprint)
            {
                var position = __instance.Position;
                var distance = (float)distanceInfo.GetValue(__instance);
                var movementState = EAudioMovementState.Sprint;
                var isTeammate = __instance.IsGroupedWithMainPlayer();
                Indicators.PrepareStep(movementState, position, distance, __instance.ProfileId, isTeammate);
            }
        }
    }
}
