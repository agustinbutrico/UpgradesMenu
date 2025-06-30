using UnityEngine;
using UnityEngine.SceneManagement;

namespace UpgradesMenu
{
    internal static class UpgradesMenuInitializer
    {
        private static GameObject upgradesMenuHolder;

        public static void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Plugin.Log.LogInfo("[UpgradesMenu] UpgradesMenuInitializer subscribed to sceneLoaded.");
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Plugin.Log.LogInfo($"[UpgradesMenu] Scene loaded: {scene.name}");

            if (scene.name == "MainMenu" && Plugin.upgradesMenuPrefab == null)
            {
                GameObject canvas = GameObject.Find("Canvas");
                if (canvas == null)
                {
                    Plugin.Log.LogError("[UpgradesMenu] Canvas not found in MainMenu.");
                    return;
                }

                Transform upgradeMenuTransform = canvas.transform.Find("UpgradeMenu");
                if (upgradeMenuTransform == null)
                {
                    Plugin.Log.LogError("[UpgradesMenu] UpgradeMenu not found under Canvas.");
                    return;
                }

                GameObject upgradesMenuClone = Object.Instantiate(upgradeMenuTransform.gameObject);
                upgradesMenuClone.name = "UpgradesMenu";
                upgradesMenuClone.SetActive(false);

                CleanMenuBeforeSave(upgradesMenuClone);

                // Create and parent under a holder
                upgradesMenuHolder = new GameObject("UpgradesMenuHolder");
                Object.DontDestroyOnLoad(upgradesMenuHolder);
                upgradesMenuClone.transform.SetParent(upgradesMenuHolder.transform, false);

                Plugin.upgradesMenuPrefab = upgradesMenuClone;

                Plugin.Log.LogInfo("[UpgradesMenu] UpgradesMenu cloned, cleaned, and stored persistently under holder.");
            }

            if (scene.name == "GameScene")
            {
                if (Plugin.upgradesMenuPrefab == null)
                {
                    Plugin.Log.LogError("[UpgradesMenu] No saved upgradesMenuPrefab found for GameScene injection.");
                    return;
                }

                GameObject gameUI = GameObject.Find("GameUI");
                if (gameUI == null)
                {
                    Plugin.Log.LogError("[UpgradesMenu] GameUI not found in GameScene.");
                    return;
                }

                GameObject upgradesMenuInstance = Object.Instantiate(Plugin.upgradesMenuPrefab, gameUI.transform);
                upgradesMenuInstance.name = "UpgradesMenu";
                upgradesMenuInstance.SetActive(false);

                // Attach global hotkey manager on first GameScene load
                if (GameObject.Find("UpgradesMenuHotkeyManager") == null)
                {
                    GameObject hotkeyManagerObj = new GameObject("UpgradesMenuHotkeyManager");
                    Object.DontDestroyOnLoad(hotkeyManagerObj);
                    var hotkeyManager = hotkeyManagerObj.AddComponent<UpgradesMenuHotkeyManager>();
                    hotkeyManager.Initialize(upgradesMenuInstance);
                    Plugin.Log.LogInfo("[UpgradesMenu] Hotkey manager created and linked to UpgradesMenu.");
                }

                CleanMenuForRun(upgradesMenuInstance);

                Plugin.Log.LogInfo("[UpgradesMenu] UpgradesMenu injected under GameUI in GameScene and prepared for the run.");
            }
        }

        private static void CleanMenuBeforeSave(GameObject upgradesMenu)
        {
            Transform slidingScreen = upgradesMenu.transform.Find("SlidingScreen");
            if (slidingScreen == null)
            {
                Plugin.Log.LogError("[UpgradesMenu] SlidingScreen not found.");
                return;
            }

            // Cache Ballista card's own Image sprite and color
            Transform ballistaTransform = slidingScreen.Find("Ballista/Ballista");
            if (ballistaTransform == null)
            {
                Plugin.Log.LogError("[UpgradesMenu] Ballista reference card not found.");
                return;
            }

            var referenceImage = ballistaTransform.GetComponent<UnityEngine.UI.Image>();
            if (referenceImage == null)
            {
                Plugin.Log.LogError("[UpgradesMenu] Ballista does not have an Image component.");
                return;
            }

            var referenceSprite = referenceImage.sprite;
            var referenceColor = referenceImage.color;
            Plugin.Log.LogInfo($"[UpgradesMenu] Cached Ballista sprite ({referenceSprite?.name ?? "null"}) and color {referenceColor}.");

            // Remove unnecessary panels
            Transform scaler = upgradesMenu.transform.Find("Scaler");
            if (scaler != null) Object.Destroy(scaler.gameObject);

            Transform keybindsUI = slidingScreen.Find("KeybindsUI");
            if (keybindsUI != null) Object.Destroy(keybindsUI.gameObject);

            Transform examples = slidingScreen.Find("Examples");
            if (examples != null) Object.Destroy(examples.gameObject);

            // Clean all cards
            foreach (Transform category in slidingScreen)
            {
                string categoryName = category.name;
                if (categoryName == "GoldPermanant" || categoryName == "ManaPermanant" ||
                    categoryName == "HealthPermanant" || categoryName == "CardPermanant")
                {
                    Plugin.Log.LogInfo($"[UpgradesMenu] Skipping category: {categoryName}");
                    continue;
                }

                foreach (Transform card in category)
                {
                    CleanCard(card, referenceSprite, referenceColor);
                }
            }

            Plugin.Log.LogInfo("[UpgradesMenu] CleanMenuBeforeSave completed: all cards cleaned and made informational.");
        }

        private static void CleanCard(Transform card, Sprite referenceSprite, Color referenceColor)
        {
            // Remove interactivity
            var button = card.GetComponent<UnityEngine.UI.Button>();
            if (button != null) Object.Destroy(button);

            var upgradeButton = card.GetComponent<UpgradeButton>();
            if (upgradeButton != null) Object.Destroy(upgradeButton);

            var eventTrigger = card.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (eventTrigger != null) Object.Destroy(eventTrigger);

            // Apply Ballista's sprite and color to card's own Image component
            var image = card.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.sprite = referenceSprite;
                image.color = referenceColor;
                Plugin.Log.LogInfo($"[UpgradesMenu] Cleaned card '{card.name}' with Ballista sprite and color.");
            }
            else
            {
                Plugin.Log.LogWarning($"[UpgradesMenu] Card '{card.name}' missing Image component; skipping sprite/color application.");
            }
        }

        private static void CleanMenuForRun(GameObject upgradesMenu)
        {
            // Optionally filter cards here based on the current run pool
            Plugin.Log.LogInfo("[UpgradesMenu] CleanMenuForRun executed.");
        }
    }
}
