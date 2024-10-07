using EFT.UI;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using acidphantasm_accessibilityindicators.IndicatorUI;

namespace acidphantasm_accessibilityindicators.Patches
{
    internal class GameWorldOnGameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        public static void PatchPostfix(GameWorld __instance)
        {
            if (Panel.IndicatorHUD == null) Panel.CreateHUD();
        }
    }
}
