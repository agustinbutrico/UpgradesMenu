using CardDataLib;
using CardsShared;
using CardRenderer;
using HarmonyLib;
using System.Collections.Generic;
using UpgradesMenu.Menu.Style;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Integrations
{
    internal class BanishCard_Patch
    {
        public static void ApplyIfAvailable(Harmony harmony)
        {
            var banishCardsType = AccessTools.TypeByName("BanishCards.CardBanishManager");
            if (banishCardsType == null) return; // Mod not present

            var methodToPatch = AccessTools.Method(banishCardsType, "BanishCard");
            if (methodToPatch == null) return;

            var postfix = typeof(BanishCard_Patch).GetMethod(nameof(Postfix));
            harmony.Patch(methodToPatch, postfix: new HarmonyMethod(postfix));
        }

        public static void Postfix()
        {
            var banishedUnlockNames = BanishCardsInterop.GetBanishedUnlockNames();
            if (banishedUnlockNames == null) return;

            var affectedUnlocks = new HashSet<string>();

            foreach (var unlockName in banishedUnlockNames)
            {
                var card = CardDataLibAPI.GetCard(unlockName);
                if (card == null) continue;

                // Add the banished card itself
                affectedUnlocks.Add(unlockName);

                // Filter cards from same category/subcategory
                var relatedCards = CardDataLibAPI.FilterCards(
                    card.Category,
                    card.Subcategory,
                    AcquisitionType.StoreUnlocksInRun | AcquisitionType.AlwaysInRun,
                    unlocked: true
                );

                // Build dependency trees
                var roots = CardTreeBuilder.BuildCardTrees(relatedCards);

                // Try to find the tree node for the banished card
                foreach (var root in roots)
                {
                    var match = FindNode(root, unlockName);
                    if (match != null)
                    {
                        var descendants = CardTreeUtils.GetAllDescendants(match);
                        foreach (var desc in descendants)
                            affectedUnlocks.Add(desc.UnlockName);
                        break; // stop once we've found it
                    }
                }
            }

            var cardStyle = StyleManager.GetCardStyle(StyleType.Banish);

            // Apply slice change
            foreach (var affected in affectedUnlocks)
            {
                var data = CardDataLibAPI.GetCard(affected);
                if (data == null) continue;

                string category = data.Category.ToString();
                string subcategory = data.Subcategory.ToString();
                string path = $"GameUI/UpgradesMenu/SlidingScreen/ActiveCards/{category}Cards/{subcategory}/{affected}";

                CardRendererAPI.SwapCardSlice(path, cardStyle.SliceUnlocked, cardStyle.IconColor, cardStyle.TextColor);
            }
        }

        private static CardTree FindNode(CardTree node, string unlockName)
        {
            if (node.Card.UnlockName == unlockName)
                return node;

            foreach (var child in node.Children)
            {
                var result = FindNode(child, unlockName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
