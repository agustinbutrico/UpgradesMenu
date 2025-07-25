using UnityEngine;
using UnityEngine.UI;

namespace UpgradesMenu.Utility
{
    internal class MonsterModeHook
    {
        internal static void HookMonsterToggle()
        {
            foreach (var toggle in Object.FindObjectsOfType<Toggle>(true))
            {
                if (toggle.name == "MonsterMode")
                {
                    // Immediately capture initial state
                    GameModeCheck.ChallengeMode.IsMonsterModeEnabled = toggle.isOn;

                    // Hook for changes
                    toggle.onValueChanged.AddListener(isOn =>
                    {
                        GameModeCheck.ChallengeMode.IsMonsterModeEnabled = isOn;
                    });

                    return;
                }
            }
        }

        private static void OnPlayClicked()
        {
            var toggleGO = GameObject.Find("Canvas/PlayMenu/Scaler/ChallengePanel/MonsterMode");
            if (toggleGO != null)
            {
                var toggle = toggleGO.GetComponent<Toggle>();
                if (toggle != null)
                {
                    GameModeCheck.ChallengeMode.IsMonsterModeEnabled = toggle.isOn;
                }
                else
                {
                    Plugin.Log.LogWarning("Toggle component not found on MonsterMode");
                }
            }
        }
    }
}
