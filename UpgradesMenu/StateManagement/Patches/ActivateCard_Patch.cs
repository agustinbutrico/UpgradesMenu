using CardDataLib;
using CardRenderer;
using HarmonyLib;
using System.Collections.Generic;
using UpgradesMenu.StateManagement.Runtime;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.StateManagement.Patches
{
    [HarmonyPatch(typeof(CardManager), "ActivateCard")]
    internal class ActivateCard_Patch
    {
        static void Prefix(CardManager __instance)
        {
            ActiveCardsInstance.SnapshotNormal(
                GetList(__instance, "availableCards")
            );
        }

        static void Postfix(CardManager __instance)
        {
            var currentNormal = GetList(__instance, "availableCards");
            var cardStyle = GetCardStyle(StyleType.Active);

            foreach (var card in ActiveCardsInstance.GetRemovedCards(currentNormal, true))
            {
                string unlockName = ActiveCardsInstance.unlockNameRef(card);

                var cardData = CardDataLibAPI.GetCard(unlockName);
                if (cardData == null) continue;

                string category = cardData.Category.ToString();
                string subcategory = cardData.Subcategory.ToString();
                string path = $"GameUI/UpgradesMenu/SlidingScreen/ActiveCards/{category}Cards/{subcategory}/{unlockName}";

                CardRendererAPI.SwapCardSlice(path, cardStyle.SliceUnlocked, cardStyle.IconColor, cardStyle.TextColor);
            }

            ActiveCardsInstance.SnapshotNormal(currentNormal);
        }

        private static List<UpgradeCard> GetList(CardManager cm, string fieldName)
        {
            return AccessTools.Field(typeof(CardManager), fieldName).GetValue(cm) as List<UpgradeCard>;
        }
    }
}
