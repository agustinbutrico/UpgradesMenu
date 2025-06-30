using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace UpgradesMenu
{
    [BepInPlugin("AgusBut.UpgradesMenu", "UpgradesMenu", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }

        public static GameObject upgradesMenuPrefab;

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            Logger.LogInfo("Loading [UpgradesMenu 1.0.0]");

            UpgradesMenuInitializer.Initialize();

            var harmony = new Harmony("AgusBut.UpgradesMenu");
            harmony.PatchAll();
        }
    }
}
