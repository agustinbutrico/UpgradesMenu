using UnityEngine;
using UnityEngine.UI;

namespace UpgradesMenu
{
    public class UpgradesMenuInGame : MonoBehaviour
    {
        private void OnEnable()
        {
            if (gameObject.name != "UpgradesMenuInGame")
            {
                Plugin.Log.LogWarning("[UpgradesMenu] This instance is not UpgradesMenuInGame, disabling to avoid affecting the original.");
                gameObject.SetActive(false);
                return;
            }

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GameScene")
            {
                Plugin.Log.LogWarning("[UpgradesMenu] Not in GameScene, disabling to avoid affecting the original.");
                gameObject.SetActive(false);
                return;
            }

            Plugin.Log.LogInfo("[UpgradesMenu] Restoring full card display (no filtering).");

            Transform slidingScreen = transform.Find("SlidingScreen");
            if (slidingScreen == null)
            {
                Plugin.Log.LogError("[UpgradesMenu] SlidingScreen not found.");
                return;
            }

            foreach (Transform category in slidingScreen)
            {
                category.gameObject.SetActive(true);

                foreach (Transform cardTransform in category)
                {
                    cardTransform.gameObject.SetActive(true);

                    var image = cardTransform.GetComponent<Image>();
                    if (image != null)
                    {
                        image.color = Plugin.ReferenceColor;
                    }
                }
            }

            Plugin.Log.LogInfo("[UpgradesMenu] Full card display restored.");
        }
    }
}
