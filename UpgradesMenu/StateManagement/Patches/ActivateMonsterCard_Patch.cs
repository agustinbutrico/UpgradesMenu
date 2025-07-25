using CardDataLib;
using CardRenderer;
using HarmonyLib;
using System.Collections.Generic;
using UpgradesMenu.Integrations;
using UpgradesMenu.Menu.Style;
using UpgradesMenu.StateManagement.Runtime;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.StateManagement.Patches
{
    [HarmonyPatch(typeof(CardManager), "ActivateMonsterCard")]
    internal class ActivateMonsterCard_Patch
    {
        static void Prefix(CardManager __instance)
        {
            ActiveCardsInstance.SnapshotMonster(
                GetList(__instance, "availableMonsterCards")
            );
        }

        static void Postfix(CardManager __instance)
        {
            var currentMonster = GetList(__instance, "availableMonsterCards");
            var cardStyle = GetCardStyle(StyleType.Monster);

            foreach (var card in ActiveCardsInstance.GetRemovedCards(currentMonster, false))
            {
                string unlockName = ActiveCardsInstance.unlockNameRef(card);
                
                var cardData = CardDataLibAPI.GetCard(unlockName);
                if (cardData == null) continue;

                string category = cardData.Category.ToString();
                string subcategory = cardData.Subcategory.ToString();
                string path = $"GameUI/UpgradesMenu/SlidingScreen/MonsterCards/{category}Cards/{subcategory}/{unlockName}";

                CardRendererAPI.SwapCardSlice(path, cardStyle.SliceUnlocked, cardStyle.IconColor, cardStyle.TextColor);
            }

            ActiveCardsInstance.SnapshotMonster(currentMonster);
        }

        private static List<UpgradeCard> GetList(CardManager cm, string fieldName)
        {
            return AccessTools.Field(typeof(CardManager), fieldName).GetValue(cm) as List<UpgradeCard>;
        }
    }
}
