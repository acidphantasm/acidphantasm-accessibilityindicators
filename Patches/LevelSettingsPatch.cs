using AccessibilityIndicators.IndicatorUI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace AccessibilityIndicators.Patches
{
    internal class LevelSettingsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LevelSettings), nameof(LevelSettings.Awake));
        }

        [PatchPostfix]
        public static void PatchPostfix(LevelSettings __instance)
        {
            if (__instance == null) return;

            Panel.NorthVector = __instance.NorthVector;
            Panel.NorthDirection = __instance.NorthDirection;
        }
    }
}
