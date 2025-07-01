using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UpgradesMenu
{
    [HarmonyPatch(typeof(CardManager), "Start")]
    internal static class CardPoolStatusLogger
    {
        static void Postfix(CardManager __instance)
        {
            var availableCards = AccessTools.Field(typeof(CardManager), "availableCards").GetValue(__instance) as List<UpgradeCard>;
            var availableMonsterCards = AccessTools.Field(typeof(CardManager), "availableMonsterCards").GetValue(__instance) as List<UpgradeCard>;

            HashSet<UpgradeCard> poolSet = new HashSet<UpgradeCard>(availableCards);
            HashSet<UpgradeCard> monsterPoolSet = new HashSet<UpgradeCard>(availableMonsterCards);

            UpgradeCard[] allCards = GameObject.FindObjectsOfType<UpgradeCard>();
            List<UpgradeCard> unlockedEligibleNotInPool = new List<UpgradeCard>();
            List<UpgradeCard> unlockedButStillLockedByPrereq = new List<UpgradeCard>();

            Plugin.Log.LogInfo("[UpgradesMenu] ==== RUN POOL AND UNLOCKABLE CARDS DUMP BEGIN ====");

            // === 1️⃣ Log RUN POOL ===
            Plugin.Log.LogInfo($"[UpgradesMenu] RUN POOL Count: {availableCards.Count}");
            for (int i = 0; i < availableCards.Count; i++)
                LogCardBasic(availableCards[i], $"[RunPool Card {i}]");

            // === 2️⃣ Log MONSTER POOL ===
            Plugin.Log.LogInfo($"[UpgradesMenu] MONSTER POOL Count: {availableMonsterCards.Count}");
            for (int i = 0; i < availableMonsterCards.Count; i++)
                LogCardBasic(availableMonsterCards[i], $"[MonsterPool Card {i}]");

            // === 3️⃣ Find unlocked cards eligible to appear next level up ===
            foreach (var card in allCards)
            {
                if (card == null) continue;

                string cardName = card.title ?? "";
                string normalizedName = cardName.Replace(" ", "");
                string unlockName = (string)AccessTools.Field(typeof(UpgradeCard), "unlockName").GetValue(card) ?? "";
                bool unlockedByDefault = (bool)AccessTools.Field(typeof(UpgradeCard), "unlockedByDefault").GetValue(card);
                int playerPrefState = string.IsNullOrEmpty(unlockName) ? -1 : PlayerPrefs.GetInt(unlockName, 0);

                // ✅ Skip cards belonging to towers not purchased
                if (TowerUnlockRegistry.TowerUnlockMapping.TryGetValue(normalizedName, out string towerUnlockName))
                {
                    int towerPurchased = PlayerPrefs.GetInt(towerUnlockName, 0);
                    if (towerPurchased == 0)
                    {
                        Plugin.Log.LogInfo($"[Skipped] Card '{cardName}' skipped because tower '{normalizedName}' is not purchased (PlayerPrefs '{towerUnlockName}' = 0).");
                        continue;
                    }
                }

                if (card.unlocked && !poolSet.Contains(card) && !monsterPoolSet.Contains(card))
                {
                    if (string.IsNullOrEmpty(unlockName) || playerPrefState == 1)
                    {
                        unlockedEligibleNotInPool.Add(card);
                    }
                    else
                    {
                        unlockedButStillLockedByPrereq.Add(card);
                    }
                }
            }

            // === 4️⃣ Log unlocked cards eligible but not in pool ===
            Plugin.Log.LogInfo($"[UpgradesMenu] UNLOCKED & ELIGIBLE BUT NOT IN POOL Count: {unlockedEligibleNotInPool.Count}");
            for (int i = 0; i < unlockedEligibleNotInPool.Count; i++)
                LogCardBasic(unlockedEligibleNotInPool[i], $"[EligibleNotInPool {i}]");

            // === 5️⃣ Log unlocked cards but still locked by prerequisites ===
            Plugin.Log.LogInfo($"[UpgradesMenu] UNLOCKED BUT STILL LOCKED BY PREREQUISITES Count: {unlockedButStillLockedByPrereq.Count}");
            for (int i = 0; i < unlockedButStillLockedByPrereq.Count; i++)
            {
                var card = unlockedButStillLockedByPrereq[i];
                string cardUnlockName = (string)AccessTools.Field(typeof(UpgradeCard), "unlockName").GetValue(card) ?? "(null)";
                int cardPlayerPrefState = string.IsNullOrEmpty(cardUnlockName) ? -1 : PlayerPrefs.GetInt(cardUnlockName, 0);

                LogCardBasic(card, $"[LockedByPrereq {i}]");
                Plugin.Log.LogInfo($"[LockedByPrereq {i}] STILL LOCKED: unlockName='{cardUnlockName}' PlayerPrefs={cardPlayerPrefState}");
            }

            Plugin.Log.LogInfo("[UpgradesMenu] ==== RUN POOL AND UNLOCKABLE CARDS DUMP END ====");
        }

        static void LogCardBasic(UpgradeCard card, string prefix)
        {
            if (card == null)
            {
                Plugin.Log.LogInfo($"{prefix} NULL");
                return;
            }

            string title = card.title ?? "(null)";
            string description = card.description ?? "(null)";
            string spriteName = card.image != null ? card.image.name : "(null)";
            string unlockName = (string)AccessTools.Field(typeof(UpgradeCard), "unlockName").GetValue(card) ?? "(null)";
            bool unlockedByDefault = (bool)AccessTools.Field(typeof(UpgradeCard), "unlockedByDefault").GetValue(card);
            int playerPrefState = string.IsNullOrEmpty(unlockName) ? -1 : PlayerPrefs.GetInt(unlockName, 0);
            string unlocks = card.unlocks != null && card.unlocks.Count > 0 ? $"[{string.Join(", ", card.unlocks.Select(u => u?.title ?? "(null)"))}]" : "[(none)]";

            Plugin.Log.LogInfo($"{prefix} Title: {title}, Desc: {description}, Sprite: {spriteName}, Unlocked: {card.unlocked}, UnlockedByDefault: {unlockedByDefault}, UnlockName: {unlockName}, PlayerPrefs: {playerPrefState}, Unlocks: {unlocks}");
        }
    }
}
