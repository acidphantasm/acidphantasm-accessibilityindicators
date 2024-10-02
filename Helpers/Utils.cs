using EFT;
using Comfort.Common;

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

    }
}
