using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using AccessibilityIndicators.IndicatorUI;
using AccessibilityIndicators.Scripts;

namespace AccessibilityIndicators.Patches
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
            if (Panel.IndicatorHUD == null) 
                Panel.CreateHUD();
        }
    }

    internal class GameWorldUnregisterPlayerPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.UnregisterPlayer));
        }

        [PatchPostfix]
        public static void PatchPostFix(IPlayer iPlayer)
        {
            var player = iPlayer as Player;
            if (player == null) 
                return;
            
            if (player.IsYourPlayer && Panel.IndicatorHUD != null) 
                Panel.Dispose();
        }
    }
}
