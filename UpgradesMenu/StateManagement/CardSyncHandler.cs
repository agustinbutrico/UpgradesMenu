using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UpgradesMenu.Menu.Logic;
using UpgradesMenu.Utility;

namespace UpgradesMenu.StateManagement
{
    internal static class CardSyncHandler
    {
        internal static void Register()
        {
            CardDataLib.Plugin.OnCardSyncComplete += HandleCardSyncComplete;
        }

        private static void HandleCardSyncComplete()
        {
            if (Plugin.UseRedMonsterColors())
            {
                ConfigManager.MonsterPanelColor.Value = "DeepRed";
                ConfigManager.MonsterCardColor.Value = "DeepRed";
            }

            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                Plugin.Instance.StartCoroutine(LoadUpgradeMenu());
            }
        }

        private static IEnumerator LoadUpgradeMenu()
        {
            bool menuReady = false;

            yield return MenuManager.LoadMenuWhenSceneIsReady(() =>
            {
                menuReady = true;
                StatusUpdater.UpdateCardsSlicesAtStart();
            });

            yield return new WaitUntil(() => menuReady);
        }

        internal static void Unregister()
        {
            CardDataLib.Plugin.OnCardSyncComplete -= HandleCardSyncComplete;
        }
    }
}
