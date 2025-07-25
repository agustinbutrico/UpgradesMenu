using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace UpgradesMenu.StateManagement.Runtime
{
    internal static class ActiveCardsInstance
    {
        private static List<UpgradeCard> prevNormal = new List<UpgradeCard>();
        private static List<UpgradeCard> prevMonster = new List<UpgradeCard>();

        internal static void SnapshotNormal(List<UpgradeCard> currentNormal)
        {
            prevNormal = new List<UpgradeCard>(currentNormal);
        }

        internal static void SnapshotMonster(List<UpgradeCard> currentMonster)
        {
            prevMonster = new List<UpgradeCard>(currentMonster);
        }

        internal static List<UpgradeCard> GetRemovedCards(List<UpgradeCard> current, bool isNormal)
        {
            var prev = isNormal ? prevNormal : prevMonster;

            var currentNames = new HashSet<string>(
                current.Select(card => unlockNameRef(card))
            );

            return prev
                .Where(card =>
                {
                    string name = unlockNameRef(card);
                    return !currentNames.Contains(name);
                })
                .ToList();
        }

        internal static bool WasCardRemoved(UpgradeCard card, List<UpgradeCard> current, bool isNormal)
        {
            return (isNormal ? prevNormal : prevMonster).Contains(card) && !current.Contains(card);
        }

        internal static readonly AccessTools.FieldRef<UpgradeCard, string> unlockNameRef =
            AccessTools.FieldRefAccess<UpgradeCard, string>("unlockName");
    }
}
