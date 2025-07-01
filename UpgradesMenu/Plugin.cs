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

        // 🪐 Hardcoded references for card unlock detection
        public static string UnlockVariant1SpriteName = "UI9SliceOrange";         // Unlocked variant 1
        public static string UnlockVariant2SpriteName = "UI9SliceBlueFilled";     // Unlocked variant 2
        public static string LockVariant1SpriteName = "UI9SliceOrangeFilled";     // Locked variant 1
        public static string LockVariant2SpriteName = "UI9SliceBlue";             // Locked variant 2

        public static Color ReferenceColor = new Color(1f, 1f, 1f, 1f);

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            UpgradesMenuInitializer.Initialize();

            GameObject go = new GameObject("CardUnlockDebugger");
            DontDestroyOnLoad(go);

            Logger.LogInfo("CardUnlockDebugger GameObject created and added to scene.");

            var harmony = new Harmony("AgusBut.UpgradesMenu");
            harmony.PatchAll();
        }
    }
}
