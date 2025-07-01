using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UpgradesMenu
{
    internal static class TowerUnlockRegistry
    {
        static readonly Dictionary<string, string> TowerUnlockMapping = new Dictionary<string, string>
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

        public static bool IsTowerPurchased(string towerName)
        {
            if (!TowerUnlockMapping.TryGetValue(towerName, out string unlockName))
                return true; // If not a tower, treat as always purchased

            int purchased = PlayerPrefs.GetInt(unlockName, 0);
            return purchased == 1;
        }
    }

}
