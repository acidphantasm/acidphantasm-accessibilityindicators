using AccessibilityIndicators.Helpers;
using AccessibilityIndicators.IndicatorUI;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace AccessibilityIndicators.Patches
{
    internal class PhraseSpeakerClassPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BaseSpeaker), nameof(BaseSpeaker.Play));
        }

        [PatchPostfix]
        static void PatchPostfix(BaseSpeaker __instance, EPhraseTrigger trigger)
        {
            Player player = Utility.GetProfileByID(__instance.Id);

            if (player == null
                || player.IsYourPlayer
                || System.Enum.IsDefined(typeof(BannedPhrases), trigger.ToString())
                || !Indicators.Enable
                || !Indicators.EnableVoicelines
                || (!player.IsAI && player.IsGroupedWithMainPlayer() && !Indicators.ShowTeammates)) return;

            var isTeammate = player.IsGroupedWithMainPlayer();
            Indicators.PrepareVoice(player.Position, player.ProfileId, isTeammate);

        }
    }
}
