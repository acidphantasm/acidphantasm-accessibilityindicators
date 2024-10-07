using EFT;
using Comfort.Common;
using acidphantasm_accessibilityindicators.IndicatorUI;
using UnityEngine;

namespace acidphantasm_accessibilityindicators.Helpers
{
    public static class Utils
    {
        public static Player GetMainPlayer()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            return gameWorld?.MainPlayer;
        }
        public static bool IsGroupedWithMainPlayer(this Player player)
        {
            var mainPlayerGroupId = GetMainPlayer().GroupId;
            return !string.IsNullOrEmpty(mainPlayerGroupId) && player.GroupId == mainPlayerGroupId;
        }

        public static float GetDistance(Vector3 from, Vector3 to)
        {
            float distance = Vector3.Distance(from, to);
            return distance;
        }
        public static float GetAngle(Vector3 originDirection)
        {
            originDirection.y = 0;
            var angle = Vector3.SignedAngle(originDirection, Panel.northVector, Vector3.up);
            if (angle >= 0) return angle;
            return angle + 360;
        }
        public static float GetLookAngle(Vector3 originAngle)
        {
            var angle = originAngle.y - Panel.northDirection;

            if (angle >= 0) return angle;
            return angle + 360;
        }

        public static float CustomInverseLerp(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }
    }
}
