using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace acidphantasm_accessibilityindicators.Patches
{
    internal class LevelSettingsNorthVectorPatch : ModulePatch
    {
        public static Vector3 northVector;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(LevelSettings), nameof(LevelSettings.NorthVector));
        }

        [PatchPostfix]
        public static void PatchPostfix(LevelSettings __instance, Vector3 __result)
        {
            if (__instance == null) return;
            Plugin.LogSource.LogInfo(__result);
            northVector = __result;
        }
    }
    internal class LevelSettingsNorthDirectionPatch : ModulePatch
    {
        public static float northDirection;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(LevelSettings), nameof(LevelSettings.NorthDirection));
        }

        [PatchPostfix]
        public static void PatchPostfix(LevelSettings __instance, float __result)
        {
            if (__instance == null) return;
            Plugin.LogSource.LogInfo(__result);
            northDirection = __result;
        }
    }
}
