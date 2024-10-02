﻿using acidphantasm_accessibilityindicators.IndicatorUI;
using acidphantasm_accessibilityindicators.Helpers;
using BepInEx;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace acidphantasm_accessibilityindicators.Patches
{
    internal class LevelSettingsNorthVectorPatch : ModulePatch
    {
        public static Vector3 northVector;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(LevelSettings), "NorthVector");
        }

        [PatchPostfix]
        static void PatchPostfix(LevelSettings __instance, ref Vector3 __result)
        {
            if (__instance == null) return;
            northVector = __result;
        }
    }
    internal class LevelSettingsNorthDirectionPatch : ModulePatch
    {
        public static float northDirection;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(LevelSettings), "NorthDirection");
        }

        [PatchPostfix]
        static void PatchPostfix(LevelSettings __instance, ref float __result)
        {
            if (__instance == null) return;
            northDirection = __result;
        }
    }
}
