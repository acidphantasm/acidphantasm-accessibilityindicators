﻿using SPT.Reflection.Patching;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using EFT;
using acidphantasm_accessibilityindicators.IndicatorUI;
using acidphantasm_accessibilityindicators.Helpers;

namespace acidphantasm_accessibilityindicators.Patches
{
    internal class FirearmControllerPatch : ModulePatch
    {
        private static FieldInfo playerInfo;

        protected override MethodBase GetTargetMethod()
        {
            playerInfo = AccessTools.Field(typeof(EFT.Player.FirearmController), "_player");
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.InitiateShot));
        }

        [PatchPostfix]
        static void PatchPostfix(Player.FirearmController __instance, Vector3 shotPosition)
        {
            if (__instance == null) return;

            Player player = (Player)playerInfo.GetValue(__instance);

            if (player.IsYourPlayer
                || !Indicators.enable
                || (!player.IsAI && Utils.IsGroupedWithMainPlayer(player) && !Indicators.showTeammates)) return;

            if (!player.IsYourPlayer) Indicators.PrepareShot(shotPosition, player.AccountId);
        }
    }
}
