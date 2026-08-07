using SPT.Reflection.Patching;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using EFT;
using AccessibilityIndicators.IndicatorUI;
using AccessibilityIndicators.Helpers;

namespace AccessibilityIndicators.Patches
{
    internal class FirearmControllerPatch : ModulePatch
    {
        private static FieldInfo playerInfo;

        protected override MethodBase GetTargetMethod()
        {
            playerInfo = AccessTools.Field(typeof(Player.FirearmController), "_player");
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.InitiateShot));
        }

        [PatchPostfix]
        static void PatchPostfix(Player.FirearmController __instance, Vector3 shotPosition)
        {
            if (__instance == null) return;

            var player = (Player)playerInfo.GetValue(__instance);

            if (player.IsYourPlayer || !Indicators.Enable || !Indicators.EnableShots || (!player.IsAI && player.IsGroupedWithMainPlayer() && !Indicators.ShowTeammates)) return;

            var isTeammate = player.IsGroupedWithMainPlayer();

            Indicators.PrepareShot(shotPosition, player.ProfileId, isTeammate);
        }
    }
}
