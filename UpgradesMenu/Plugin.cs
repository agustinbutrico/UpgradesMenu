using BepInEx;
using HarmonyLib;
using UpgradesMenu.Integrations;
using UpgradesMenu.Menu.Logic;
using UpgradesMenu.StateManagement.Runtime;
using UpgradesMenu.StateManagement;
using UpgradesMenu.Utility;

namespace UpgradesMenu
{
    [BepInPlugin("AgusBut.UpgradesMenu", "UpgradesMenu", "1.0.0")]
    [BepInDependency("AgusBut.CardDataLib")]
    [BepInDependency("AgusBut.CardRenderer")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            ConfigManager.Initialize(this);
            SceneHooks.Register();
            CardSyncHandler.Register();

            var harmony = new Harmony("AgusBut.UpgradesMenu");
            harmony.PatchAll();

            BanishCard_Patch.ApplyIfAvailable(harmony);

            Logger.LogInfo("UpgradesMenu loaded successfully.");
        }

        private void Update()
        {
            MenuHelper.HandleMenuInput(ConfigManager.ToggleKey.Value);
        }

        private void OnDestroy()
        {
            CardSyncHandler.Unregister();
        }

        internal static bool UseRedMonsterColors()
        {
            return RedMonsterCardsInterop.IsModPresent() && !ConfigManager.OverrideRedMonsterCards.Value;
        }
    }
}
