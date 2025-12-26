using System.Collections;
using UnityEngine;

namespace UpgradesMenu.Menu.Logic
{
    internal class Panels
    {
        internal static GameObject CreatePanel(string name, Transform parent, Vector2? anchoredPos = null)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            if (anchoredPos.HasValue && go.TryGetComponent(out RectTransform rect))
                rect.anchoredPosition = anchoredPos.Value;
            return go;
        }

        internal static IEnumerator GetPanelDimensions(string menuPath, string panelName, System.Action<Vector2> onComplete)
        {
            yield return null; // Wait one frame

            var panelGO = GameObject.Find($"{menuPath}/{panelName}");
            if (panelGO == null)
            {
                Plugin.Log.LogError($"{panelName} panel not found at path {menuPath}/{panelName}");
                onComplete?.Invoke(Vector2.zero);
                yield break;
            }

            RectTransform panelRect = panelGO.GetComponent<RectTransform>();
            if (panelRect == null)
            {
                Plugin.Log.LogError($"RectTransform not found on {panelName}");
                onComplete?.Invoke(Vector2.zero);
                yield break;
            }

            if (panelRect.childCount == 0)
            {
                onComplete?.Invoke(Vector2.zero);
                yield break;
            }

            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;

            foreach (RectTransform child in panelRect)
            {
                Vector2 anchoredPos = child.anchoredPosition;
                Vector2 size = child.sizeDelta;

                float left = anchoredPos.x;
                float right = anchoredPos.x + size.x;
                float top = anchoredPos.y;
                float bottom = anchoredPos.y - size.y;

                if (left < minX) minX = left;
                if (right > maxX) maxX = right;
                if (top > maxY) maxY = top;
                if (bottom < minY) minY = bottom;
            }

            float width = maxX - minX;
            float height = maxY - minY;

            onComplete?.Invoke(new Vector2(width, height));
        }
    }
}
