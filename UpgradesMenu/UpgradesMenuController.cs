using HarmonyLib;
using UnityEngine;

namespace UpgradesMenu
{
    internal class UpgradesMenuController : MonoBehaviour
    {
        private bool isVisible = false;

        void Awake()
        {
            Plugin.Log.LogInfo("[UpgradesMenu] UpgradesMenuController attached.");
            gameObject.SetActive(false); // start hidden
            Plugin.Log.LogInfo("[UpgradesMenu] UpgradesMenu hidden on Awake.");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                isVisible = !isVisible;
                gameObject.SetActive(isVisible);
                Plugin.Log.LogInfo($"[UpgradesMenu] UpgradesMenu visibility toggled to: {isVisible}");
            }
        }
    }
}
