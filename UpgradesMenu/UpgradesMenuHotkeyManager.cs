using UnityEngine;

namespace UpgradesMenu
{
    internal class UpgradesMenuHotkeyManager : MonoBehaviour
    {
        private GameObject upgradesMenuInstance;

        public void Initialize(GameObject upgradesMenu)
        {
            upgradesMenuInstance = upgradesMenu;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (upgradesMenuInstance != null)
                {
                    bool newState = !upgradesMenuInstance.activeSelf;
                    upgradesMenuInstance.SetActive(newState);
                    Plugin.Log.LogInfo($"[UpgradesMenu] Toggled UpgradesMenu to {(newState ? "Active" : "Inactive")}.");
                }
                else
                {
                    Plugin.Log.LogError("[UpgradesMenu] HotkeyManager has no reference to UpgradesMenu.");
                }
            }
        }
    }
}
