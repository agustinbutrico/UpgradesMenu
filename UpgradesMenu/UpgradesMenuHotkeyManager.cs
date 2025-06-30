using UnityEngine;

namespace UpgradesMenu
{
    internal class UpgradesMenuHotkeyManager : MonoBehaviour
    {
        private GameObject upgradesMenuInstance;

        public void Initialize(GameObject upgradesMenu)
        {
            upgradesMenuInstance = upgradesMenu;
            Plugin.Log.LogInfo("[UpgradesMenu] Hotkey manager initialized with UpgradesMenu reference.");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (upgradesMenuInstance != null)
                {
                    bool newState = !upgradesMenuInstance.activeSelf;
                    upgradesMenuInstance.SetActive(newState);
                    Plugin.Log.LogInfo($"[UpgradesMenu] UpgradesMenu toggled to {(newState ? "Visible" : "Hidden")} with U key.");
                }
                else
                {
                    Plugin.Log.LogError("[UpgradesMenu] HotkeyManager has no reference to UpgradesMenu.");
                }
            }
        }
    }
}
