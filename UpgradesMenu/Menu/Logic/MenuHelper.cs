using UnityEngine;
using UpgradesMenu.Menu.UI.Components;
using UpgradesMenu.Utility;

namespace UpgradesMenu.Menu.Logic
{
    internal static class MenuHelper
    {
        private static bool firstOpenDone = false; // Tracks if the menu has been opened once in this run.

        internal static void HandleMenuInput(KeyCode toggleKey)
        {
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleMenu();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
            }
        }

        internal static void CloseMenu()
        {
            GameObject slidingScreen = GameObject.Find("GameUI/UpgradesMenu/SlidingScreen");
            GameObject panel = GameObject.Find("GameUI/UpgradesMenu/Panel");

            if (slidingScreen == null || panel == null)
                return;

            if (slidingScreen.activeSelf || panel.activeSelf)
            {
                slidingScreen.SetActive(false);
                panel.SetActive(false);
            }
        }

        internal static void ResetFirstOpen()
        {
            firstOpenDone = false;
        }

        internal static void ToggleMenu()
        {
            GameObject slidingScreen = GameObject.Find("GameUI/UpgradesMenu/SlidingScreen");
            GameObject panel = GameObject.Find("GameUI/UpgradesMenu/Panel");

            if (slidingScreen == null || panel == null)
                return;

            bool isActive = slidingScreen.activeSelf || panel.activeSelf;

            if (!isActive && !firstOpenDone)
            {
                // Set zoomed-out state
                var zoomHandler = slidingScreen.GetComponent<SlidingScreenZoomHandler>();
                zoomHandler?.SetZoom(ConfigManager.DefaultZoom.Value);

                RectTransform slidingRect = slidingScreen.GetComponent<RectTransform>();
                slidingRect.anchoredPosition = new Vector2(-1335f, -830f);

                firstOpenDone = true;
            }

            slidingScreen.SetActive(!isActive);
            panel.SetActive(!isActive);
        }
    }
}
