using UnityEngine;
using UnityEngine.UI;

namespace UpgradesMenu.Utility
{
    internal class Layers
    {
        internal static void AddInvisibleBackground(GameObject slidingScreenGO)
        {
            GameObject bg = new GameObject("Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bg.transform.SetParent(slidingScreenGO.transform, false);
            bg.transform.SetSiblingIndex(0); // Place it behind all other panels

            RectTransform rect = bg.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = GetSlidingScreen10xContentSize(slidingScreenGO);
            rect.anchoredPosition = new Vector2(0, 0);

            var img = bg.GetComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0f);
            img.raycastTarget = true; // Must be true to receive scroll events
        }

        private static Vector2 GetSlidingScreen10xContentSize(GameObject slidingScreenGO)
        {
            RectTransform screenRect = slidingScreenGO.GetComponent<RectTransform>();
            if (screenRect == null || screenRect.childCount == 0)
                return Vector2.zero;

            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(slidingScreenGO.transform);
            Vector3 size = bounds.size;

            Vector2 dimensions = new Vector2(10 * size.x, 10 * size.y);

            return dimensions;
        }

    }
}
