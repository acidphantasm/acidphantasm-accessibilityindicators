using EFT;
using Comfort.Common;
using AccessibilityIndicators.IndicatorUI;
using UnityEngine;

namespace AccessibilityIndicators.Helpers
{
    public enum BannedPhrases
    {
        OnBeingHurtDissapoinment,
        OnBeingHurt,
        OnBreath,
        OnDeath,
    }
    public enum VerticalityValues
    {
        Above,
        Below,
        Neither,
    }

    public static class Utility
    {
        public static Player GetMainPlayer()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            return gameWorld?.MainPlayer;
        }

        public static Player GetProfileByID(int id)
        {
            var gameWorld = Singleton<GameWorld>.Instance;

            foreach (Player player in gameWorld.allAlivePlayersByID.Values)
            {
                if (player.Id == id)
                {
                    return player;
                }
            }
            return null;
        }
        public static bool IsGroupedWithMainPlayer(this Player player)
        {
            var mainPlayerGroupId = GetMainPlayer().GroupId;
            
            return !string.IsNullOrEmpty(mainPlayerGroupId) && player.GroupId == mainPlayerGroupId;
        }

        public static float GetDistance(Vector3 from, Vector3 to)
        {
            var distance = Vector3.Distance(from, to);
            
            return distance;
        }
        public static float GetAngle(Vector3 originDirection)
        {
            originDirection.y = 0;
            
            var angle = Vector3.SignedAngle(originDirection, Panel.NorthVector, Vector3.up);
            
            if (angle >= 0) 
                return angle;
            
            return angle + 360;
        }
        public static float GetLookAngle(Vector3 originAngle)
        {
            var angle = originAngle.y - Panel.NorthDirection;

            if (angle >= 0) 
                return angle;
            
            return angle + 360;
        }

        public static float CustomInverseLerp(float oldMin, float oldMax, float newMin, float newMax, float oldValue)
        {
            if (oldValue > oldMax) oldValue = oldMax;
            if (oldValue < oldMin) oldValue = oldMin;

            var oldRange = (oldMax - oldMin);
            var newRange = (newMax - newMin);
            var newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;

            return (newValue);
        }

        public static VerticalityValues AboveOrBelowCheck(float playerPosition, float soundPosition)
        {
            if (soundPosition > playerPosition) 
                return Mathf.Abs(soundPosition - playerPosition) >= 2f ? VerticalityValues.Above : VerticalityValues.Neither;
            
            if (playerPosition > soundPosition) 
                return Mathf.Abs(playerPosition - soundPosition) >= 2f ? VerticalityValues.Below : VerticalityValues.Neither;
            
            return VerticalityValues.Neither;
        }
    }
}
