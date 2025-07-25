using UnityEngine;
using UnityEngine.EventSystems;
using UpgradesMenu.Utility;

namespace UpgradesMenu.Menu.UI.Components
{
    public class SlidingScreenZoomHandler : MonoBehaviour, IScrollHandler
    {
        public RectTransform targetToZoom;
        private float scale = 1f;
        private float targetScale = 1f;

        public void OnScroll(PointerEventData eventData)
        {
            float scroll = eventData.scrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                float relativeSpeed = ConfigManager.ZoomSpeed.Value / 50 * scale;
                targetScale = Mathf.Clamp(
                    scale + scroll * relativeSpeed,
                    ConfigManager.MinZoom.Value,
                    ConfigManager.MaxZoom.Value
                );
            }
        }

        private void Update()
        {
            scale = Mathf.Lerp(scale, targetScale, Time.deltaTime * 10f);
            targetToZoom.localScale = Vector3.one * scale;
        }

        public void SetZoom(float value)
        {
            scale = value;
            targetScale = value;
            if (targetToZoom != null)
                targetToZoom.localScale = Vector3.one * value;
        }
    }
}
