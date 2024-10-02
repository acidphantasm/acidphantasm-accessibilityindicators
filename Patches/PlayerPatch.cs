using acidphantasm_accessibilityindicators.IndicatorUI;
using Audio.Data;
using EFT;
using EFT.NextObservedPlayer;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EFT.Player;

namespace acidphantasm_accessibilityindicators.Patches
{
    internal class PlayerDefaultPlayPatch : ModulePatch
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
            if (__instance == null || __instance.IsYourPlayer) return;

            Vector3 position = __instance.Position;
            float distance = (float)distanceInfo.GetValue(__instance);

            Indicators.PrepareStep(movementState, position, distance, __instance.AccountId);
        }
    }
    internal class PlayerPlayStepSoundPatch : ModulePatch
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
            if (__instance == null || __instance.IsYourPlayer) return;

            Vector3 position = __instance.Position;
            float distance = (float)distanceInfo.GetValue(__instance);
            EAudioMovementState eaudioMovementState = ((__instance.Pose == EPlayerPose.Duck) ? EAudioMovementState.Duck : EAudioMovementState.Run);

            Indicators.PrepareStep(eaudioMovementState, position, distance, __instance.AccountId);
        }
    }
    internal class PlayerMethod50Patch : ModulePatch
    {
        private static FieldInfo distanceInfo;
        protected override MethodBase GetTargetMethod()
        {
            distanceInfo = AccessTools.Field(typeof(Player), "_distance");
            return AccessTools.Method(typeof(Player), nameof(Player.method_50));
        }

        [PatchPostfix]
        static void PatchPostfix(Player __instance)
        {
            if (__instance == null || __instance.IsYourPlayer) return;

            if (__instance.CurrentState.Name is EPlayerState.Sprint)
            {
                Vector3 position = __instance.Position;
                float distance = (float)distanceInfo.GetValue(__instance);
                var movementState = EAudioMovementState.Sprint;

                Indicators.PrepareStep(movementState, position, distance, __instance.AccountId);
            }

            
        }
    }
}
