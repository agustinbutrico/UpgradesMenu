using UnityEngine;
using UnityEngine.UI;
using UpgradesMenu.Menu.Logic;
using UpgradesMenu.Utility;

namespace UpgradesMenu.Menu.UI.Buttons
{
    internal class MenuToggle
    {
        internal static void CreateUpgradesMenuToggleButton(GameObject upgradesMenuGO)
        {
            // Shift the UpgradesMenu to the top right corner
            RectTransform upgradesMenuRect = upgradesMenuGO.GetComponent<RectTransform>();
            if (upgradesMenuRect != null)
            {
                upgradesMenuRect.anchorMin = new Vector2(1f, 1f);
                upgradesMenuRect.anchorMax = new Vector2(1f, 1f);
                upgradesMenuRect.pivot = new Vector2(1f, 1f);
                upgradesMenuRect.anchoredPosition = Vector2.zero;
            }
            // Shift SlidingScreen and Panel in opposite direction of new pivot
            string slidingScreenPath = "GameUI/UpgradesMenu/SlidingScreen";
            GameObject slidingScreenGO = GameObject.Find(slidingScreenPath);
            if (slidingScreenGO != null && slidingScreenGO.TryGetComponent(out RectTransform slidingScreenRect))
            {
                slidingScreenRect.anchorMin = new Vector2(1f, 1f);
                slidingScreenRect.anchorMax = new Vector2(1f, 1f);
                slidingScreenRect.pivot = new Vector2(1f, 1f);
                slidingScreenRect.anchoredPosition = new Vector2(-1500f, -400f);
            }

            GameObject panelGO = GameObject.Find("GameUI/UpgradesMenu/Panel");
            if (panelGO != null && panelGO.TryGetComponent(out RectTransform panelRect))
            {
                panelRect.anchorMin = new Vector2(1f, 1f);
                panelRect.anchorMax = new Vector2(1f, 1f);
                panelRect.pivot = new Vector2(1f, 1f);
                panelRect.anchoredPosition = new Vector2(0f, 0f);
            }

            // Create the toggle button inside UpgradesMenu
            GameObject toggleButton = new GameObject("ToggleButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            toggleButton.transform.SetParent(upgradesMenuGO.transform, false);

            // Load sprites
            Sprite normalSprite = SpriteHelper.FindSpriteByName("UI9SliceOrangeFilled");
            Sprite hoverSprite = SpriteHelper.FindSpriteByName("UI9SliceOrange");

            // Setup background image
            var image = toggleButton.GetComponent<Image>();
            if (normalSprite != null)
            {
                image.sprite = normalSprite;
                image.type = Image.Type.Sliced;
            }
            else
            {
                image.color = ColorsHelper.DeepOrange;
            }

            // Size and position
            var rect = toggleButton.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(55, 55);
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-30f, -30f);

            // Setup hover transition
            var button = toggleButton.GetComponent<Button>();
            button.transition = Selectable.Transition.SpriteSwap;

            var spriteState = new SpriteState();
            if (hoverSprite != null)
                spriteState.highlightedSprite = hoverSprite;
            button.spriteState = spriteState;


            // Add an icon/image inside the button
            GameObject iconGO = new GameObject("Icon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            iconGO.transform.SetParent(toggleButton.transform, false);

            // Set icon sprite
            var iconImage = iconGO.GetComponent<Image>();
            Sprite iconSprite = SpriteHelper.FindSpriteByName("Book");
            if (iconSprite != null)
            {
                iconImage.sprite = iconSprite;
                iconImage.preserveAspect = true;
                iconImage.color = ColorsHelper.DeepOrange;
            }

            // Icon size and position (centered)
            var iconRect = iconGO.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.pivot = new Vector2(0.5f, 0.5f);
            iconRect.anchoredPosition = Vector2.zero;
            iconRect.sizeDelta = new Vector2(50f, 50f);

            // Setup click behavior
            toggleButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                MenuHelper.ToggleMenu();
            });
        }
    }
}
