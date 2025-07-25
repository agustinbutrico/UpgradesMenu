using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UpgradesMenu.Utility;

namespace UpgradesMenu.StateManagement.Runtime
{
    internal static class SceneHooks
    {
        internal static void Register()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "MainMenu")
            {
                Plugin.Instance.StartCoroutine(DelayButtonHook());
            }
        }

        private static IEnumerator DelayButtonHook()
        {
            yield return new WaitUntil(() => GameObject.Find("Canvas/PlayMenu/Scaler/ChallengePanel/MonsterMode") != null);
            MonsterModeHook.HookMonsterToggle();
        }
    }
}
