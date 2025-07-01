using System.Collections.Generic;
using UnityEngine;

namespace UpgradesMenu
{
    internal static class UnlockCategoriesRegistry
    {
        // Towers that require purchase checks
        public static readonly Dictionary<string, string> TowerUnlockMapping = new Dictionary<string, string>
        {
            { "Ballista", "Ballista" },
            { "Mortar", "Mortar" },
            { "TeslaCoil", "Tesla" },
            { "FrostKeep", "FrostKeep" },
            { "FlameThrower", "FlameThrower" },
            { "PoisonSprayer", "PoisonSprayer" },
            { "Radar", "Radar" },
            { "Obelisk", "Obelisk" },
            { "ParticleCannon", "Particle Cannon" },
            { "Shredder", "Shredder" },
            { "Encampment", "Encampment" },
            { "Lookout", "Lookout" },
            { "VampireLair", "VampireLair" },
            { "Cannon", "Cannon" },
            { "Monument", "Monument" }
        };

        // Categories that do not require tower purchase checks
        public static readonly HashSet<string> CategoriesNoPrereq = new HashSet<string>
        {
            "Buildings",
            "DOT",
            "Monster Stuff"
        };

        // Permanent categories that should always be shown
        public static readonly HashSet<string> PermanentCategories = new HashSet<string>
        {
            "GoldPermanant",
            "ManaPermanant",
            "HealthPermanant",
            "CardPermanant"
        };

        public static bool IsTowerRequiredAndPurchased(string categoryName)
        {
            if (TowerUnlockMapping.TryGetValue(categoryName, out string unlockName))
            {
                int purchased = PlayerPrefs.GetInt(unlockName, 0);
                Plugin.Log.LogInfo($"[UnlockCheck] Tower '{categoryName}' requires '{unlockName}', PlayerPrefs = {purchased}");
                return purchased == 1;
            }

            Plugin.Log.LogInfo($"[UnlockCheck] Category '{categoryName}' does not require tower purchase.");
            return true; // No prerequisite required
        }

        public static bool IsPermanentCategory(string categoryName)
        {
            return PermanentCategories.Contains(categoryName);
        }

        public static bool IsNoPrereqCategory(string categoryName)
        {
            return CategoriesNoPrereq.Contains(categoryName);
        }
    }
}
