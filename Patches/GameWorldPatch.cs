using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
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
        public static void PatchPostfix()
        {
            if (Panel.IndicatorHUD == null) Panel.CreateHUD();
        }
    }
}
