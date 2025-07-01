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

                // Temporarily deactivate to prevent cloning side effects
                bool originalActive = upgradeMenuTransform.gameObject.activeSelf;
                upgradeMenuTransform.gameObject.SetActive(false);

                // Clone safely
                GameObject upgradesMenuClone = Object.Instantiate(upgradeMenuTransform.gameObject);
                upgradesMenuClone.name = "UpgradesMenuInGame";
                upgradesMenuClone.SetActive(false);

                // Reactivate original
                upgradeMenuTransform.gameObject.SetActive(originalActive);

                // Move under holder for persistence
                upgradesMenuHolder = new GameObject("UpgradesMenuHolder");
                Object.DontDestroyOnLoad(upgradesMenuHolder);
                upgradesMenuClone.transform.SetParent(upgradesMenuHolder.transform, false);

                // Clean without affecting original
                PrepareMenuPrefabForSaving(upgradesMenuClone);

                Plugin.upgradesMenuPrefab = upgradesMenuClone;
                Plugin.Log.LogInfo("[UpgradesMenu] UpgradesMenu cloned, cleaned, and stored persistently.");
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
                upgradesMenuInstance.name = "UpgradesMenuInGame"; // ✅ Use correct name
                upgradesMenuInstance.SetActive(false);

                if (upgradesMenuInstance.GetComponent<UpgradesMenuInGame>() == null)
                {
                    upgradesMenuInstance.AddComponent<UpgradesMenuInGame>();
                    Plugin.Log.LogInfo("[UpgradesMenu] Attached UpgradesMenu.cs to UpgradesMenuInGame instance.");
                }

                if (GameObject.Find("UpgradesMenuHotkeyManager") == null)
                {
                    GameObject hotkeyManagerObj = new GameObject("UpgradesMenuHotkeyManager");
                    Object.DontDestroyOnLoad(hotkeyManagerObj);
                    var hotkeyManager = hotkeyManagerObj.AddComponent<UpgradesMenuHotkeyManager>();
                    hotkeyManager.Initialize(upgradesMenuInstance);
                    Plugin.Log.LogInfo("[UpgradesMenu] Hotkey manager created and linked to UpgradesMenuInGame.");
                }

                CleanMenuForRun(upgradesMenuInstance);
            }
        }

        private static void PrepareMenuPrefabForSaving(GameObject upgradesMenu)
        {
            Transform slidingScreen = upgradesMenu.transform.Find("SlidingScreen");
            if (slidingScreen == null)
            {
                Plugin.Log.LogError("[UpgradesMenu] SlidingScreen not found.");
                return;
            }

            // Remove unnecessary UI
            Transform scaler = upgradesMenu.transform.Find("Scaler");
            if (scaler != null) Object.Destroy(scaler.gameObject);

            Transform keybindsUI = slidingScreen.Find("KeybindsUI");
            if (keybindsUI != null) Object.Destroy(keybindsUI.gameObject);

            Transform examples = slidingScreen.Find("Examples");
            if (examples != null) Object.Destroy(examples.gameObject);

            Transform goldPermanent = slidingScreen.Find("GoldPermanant");
            if (goldPermanent != null) Object.Destroy(goldPermanent.gameObject);

            Transform manaPermanent = slidingScreen.Find("ManaPermanant");
            if (manaPermanent != null) Object.Destroy(manaPermanent.gameObject);

            Transform healthPermanent = slidingScreen.Find("HealthPermanant");
            if (healthPermanent != null) Object.Destroy(healthPermanent.gameObject);

            Transform cardPermanent = slidingScreen.Find("CardPermanant");
            if (cardPermanent != null) Object.Destroy(cardPermanent.gameObject);

            foreach (Transform category in slidingScreen)
            {
                foreach (Transform card in category)
                {
                    CleanCard(card);
                }
            }

            Plugin.Log.LogInfo("[UpgradesMenu] CleanMenuBeforeSave completed without affecting the original.");
        }

        private static void CleanCard(Transform card)
        {
            var button = card.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.interactable = false;
            }

            var upgradeButton = card.GetComponent<UpgradeButton>();
            if (upgradeButton != null)
            {
                upgradeButton.enabled = false;
            }

            var eventTrigger = card.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (eventTrigger != null)
            {
                eventTrigger.enabled = false;
            }

            var image = card.GetComponent<UnityEngine.UI.Image>();
            if (image != null && image.sprite != null)
            {
                string spriteName = image.sprite.name;
                Sprite replacementSprite = null;

                if (spriteName == Plugin.UnlockVariant1SpriteName)
                {
                    replacementSprite = FindSpriteByName(Plugin.LockVariant1SpriteName);
                    Plugin.Log.LogInfo($"[UpgradesMenu] Swapping {Plugin.UnlockVariant1SpriteName} -> {Plugin.LockVariant1SpriteName} on '{card.name}'.");
                }
                else if (spriteName == Plugin.LockVariant1SpriteName)
                {
                    replacementSprite = FindSpriteByName(Plugin.UnlockVariant1SpriteName);
                    Plugin.Log.LogInfo($"[UpgradesMenu] Swapping {Plugin.LockVariant1SpriteName} -> {Plugin.UnlockVariant1SpriteName} on '{card.name}'.");
                }
                else if (spriteName == Plugin.UnlockVariant2SpriteName)
                {
                    replacementSprite = FindSpriteByName(Plugin.LockVariant2SpriteName);
                    Plugin.Log.LogInfo($"[UpgradesMenu] Swapping {Plugin.UnlockVariant2SpriteName} -> {Plugin.LockVariant2SpriteName} on '{card.name}'.");
                }
                else if (spriteName == Plugin.LockVariant2SpriteName)
                {
                    replacementSprite = FindSpriteByName(Plugin.UnlockVariant2SpriteName);
                    Plugin.Log.LogInfo($"[UpgradesMenu] Swapping {Plugin.LockVariant2SpriteName} -> {Plugin.UnlockVariant2SpriteName} on '{card.name}'.");
                }

                if (replacementSprite != null)
                {
                    image.sprite = replacementSprite;
                }

                // Apply unlock highlight color consistently
                image.color = Plugin.ReferenceColor;
            }
            else
            {
                Plugin.Log.LogWarning($"[UpgradesMenu] Card '{card.name}' missing Image component or sprite; skipping.");
            }
        }


        private static Sprite FindSpriteByName(string spriteName)
        {
            foreach (var sprite in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                if (sprite.name == spriteName)
                {
                    return sprite;
                }
            }
            return null;
        }

        private static void CleanMenuForRun(GameObject upgradesMenu)
        {
            Plugin.Log.LogInfo("[UpgradesMenu] CleanMenuForRun executed.");
        }
    }
}
