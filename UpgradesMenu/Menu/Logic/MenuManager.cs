using CardRenderer;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UpgradesMenu.Menu.UI.Buttons;
using UpgradesMenu.Menu.UI.Components;
using UpgradesMenu.Menu.UI.PanelsLoader;
using UpgradesMenu.Utility;

namespace UpgradesMenu.Menu.Logic
{
    internal class MenuManager
    {
        internal static IEnumerator LoadMenuWhenSceneIsReady(System.Action onComplete = null)
        {
            yield return CardRendererAPI.WaitForCardAspect("DefaultHorizontalCard");
            yield return CardRendererAPI.WaitForMenuAspect("DefaultMenu");

            GameObject parentPanel = GameObject.Find("GameUI");
            if (parentPanel == null)
            {
                Plugin.Log.LogError($"GameUI 'GameUI' not found. Cannot proceed.");
                yield break;
            }

            // Destroy existing UpgradesMenu if it exists so status is reset
            GameObject upgradesMenuGO = GameObject.Find("GameUI/UpgradesMenu");
            if (upgradesMenuGO != null)
            {
                Object.Destroy(upgradesMenuGO);
                yield return null;
            }

            // Creates the UpgradesMenu
            CardRendererAPI.NewMenu("GameUI", "UpgradesMenu");

            // Add toggle button for UpgradesMenu
            GameObject upgradesMenuWithContentGO = GameObject.Find("GameUI/UpgradesMenu");
            MenuToggle.CreateUpgradesMenuToggleButton(upgradesMenuWithContentGO);

            string slidingScreenPath = "GameUI/UpgradesMenu/SlidingScreen";

            GameObject slidingScreenGO = GameObject.Find(slidingScreenPath);
            if (slidingScreenGO == null)
            {
                Plugin.Log.LogError($"SlidingScreen not found at path '{slidingScreenPath}'");
                yield break;
            }

            // Prevent TimeScale = 1 or loading finish until menu loads
            Time.timeScale = 0f;

            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            // Hook panels to the UpgradesMenu
            yield return ActiveCards.SetupActiveCards(slidingScreenGO, $"{slidingScreenPath}/ActiveCards");
            yield return PassiveCards.SetupPassiveCards(slidingScreenGO, $"{slidingScreenPath}/PassiveCards");
            if (GameModeCheck.ChallengeMode.IsMonsterModeEnabled)
            {
                yield return MonsterCards.SetupMonsterCards(slidingScreenGO, slidingScreenPath);
            }

            // Add zoom logic
            var zoomHandler = slidingScreenGO.AddComponent<SlidingScreenZoomHandler>();
            zoomHandler.targetToZoom = slidingScreenGO.GetComponent<RectTransform>();

            // Center content
            CenterPanelsInSlidingScreen(slidingScreenGO.GetComponent<RectTransform>());

            // Add invisible background
            Layers.AddInvisibleBackground(slidingScreenGO);

            MenuHelper.CloseMenu();

            MenuHelper.ResetFirstOpen();

            // Resume normal time flow
            Time.timeScale = 1f;

            onComplete?.Invoke();
        }

        internal static void CenterPanelsInSlidingScreen(RectTransform slidingScreen)
        {
            string ballistaPath = "GameUI/UpgradesMenu/SlidingScreen/ActiveCards/TowerCards/Ballista";
            GameObject ballistaGO = GameObject.Find(ballistaPath);
            if (ballistaGO == null)
            {
                Plugin.Log.LogWarning("Ballista not found, using default centering.");
                return;
            }

            RectTransform ballistaRect = ballistaGO.GetComponent<RectTransform>();
            if (ballistaRect == null)
            {
                Plugin.Log.LogWarning("Ballista RectTransform missing.");
                return;
            }

            // Convert Ballista's bottom-right local corner to SlidingScreen space
            Vector3[] corners = new Vector3[4];
            ballistaRect.GetWorldCorners(corners);
            Vector3 worldBottomRight = corners[3];

            Vector3 localBottomRight = slidingScreen.InverseTransformPoint(worldBottomRight);
            Vector2 offset = new Vector2(localBottomRight.x + 35f, localBottomRight.y - 60f); // Little adjust

            // Shift all children so Ballista's bottom-right moves to (0, 0)
            foreach (Transform child in slidingScreen)
            {
                if (child.name == "Background") continue;
                if (child is RectTransform rect)
                {
                    rect.anchoredPosition -= offset;
                }
            }
        }
    }
}
