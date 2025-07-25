using CardDataLib;
using CardsShared;
using CardRenderer;
using UpgradesMenu.Menu.Style;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.StateManagement
{
    internal class StatusUpdater
    {
        internal static void UpdateCardsSlicesAtStart()
        {
            // Ballista is always active, so we need to set its slice
            var ballistaCard = CardDataLibAPI.GetCard("Ballista", "Ballista");
            string ballistaPath = "GameUI/UpgradesMenu/SlidingScreen/ActiveCards/TowerCards/Ballista/Ballista";
            if (ballistaCard.Unlocked)
            {
                var cardStyle =  StyleManager.GetCardStyle(StyleType.Active);
                CardRendererAPI.SwapCardSlice(ballistaPath, cardStyle.SliceUnlocked, cardStyle.IconColor, cardStyle.TextColor);
            }
            var allTowers = new[]
            {
                Subcategory.Ballista, Subcategory.Mortar, Subcategory.TeslaCoil,
                Subcategory.FrostKeep, Subcategory.FlameThrower, Subcategory.PoisonSprayer,
                Subcategory.Shredder, Subcategory.Encampment, Subcategory.Lookout,
                Subcategory.VampireLair, Subcategory.Cannon, Subcategory.Monument,
                Subcategory.Radar, Subcategory.Obelisk, Subcategory.ParticleCannon
            };

            foreach (var subcategory in allTowers)
            {
                string towerName = subcategory.ToString();
                var towerCard = CardDataLibAPI.GetCard(towerName, towerName);
                if (towerCard == null || !towerCard.Unlocked)
                    continue;

                var pasiveTowerCards = CardDataLibAPI.FilterCards(
                    Category.Tower,
                    subcategory,
                    AcquisitionType.StoreAppliesPermanent
                );

                foreach (var card in pasiveTowerCards)
                {
                    string cardPath = $"GameUI/UpgradesMenu/SlidingScreen/PassiveCards/TowerCards/{towerName}/{card.UnlockName}";

                    var cardStyle = StyleManager.GetCardStyle(StyleType.Passive);

                    if (card.Unlocked)
                        CardRendererAPI.SwapCardSlice(cardPath, cardStyle.SliceUnlocked, cardStyle.IconColor, cardStyle.TextColor);
                    else
                        CardRendererAPI.SwapCardSlice(cardPath, cardStyle.SliceLocked, cardStyle.IconColor, cardStyle.TextColor);
                }
            }

            var allBuffs = new[]
            {
                Subcategory.DrawBuff, Subcategory.ManaBuff,
                Subcategory.GoldBuff, Subcategory.TowerBuff
            };

            foreach (var subcategory in allBuffs)
            {
                var buffName = subcategory.ToString();
                var pasiveBuffCards = CardDataLibAPI.FilterCards(
                    Category.Buff,
                    subcategory,
                    AcquisitionType.StoreAppliesPermanent
                );

                foreach (var card in pasiveBuffCards)
                {
                    string cardPath = $"GameUI/UpgradesMenu/SlidingScreen/PassiveCards/BuffCards/{buffName}/{card.UnlockName}";

                    var cardStyle = StyleManager.GetCardStyle(StyleType.Passive);

                    if (card.Unlocked)
                        CardRendererAPI.SwapCardSlice(cardPath, cardStyle.SliceUnlocked, cardStyle.IconColor, cardStyle.TextColor);
                    else
                        CardRendererAPI.SwapCardSlice(cardPath, cardStyle.SliceLocked, cardStyle.IconColor, cardStyle.TextColor);
                }
            }
        }
    }
}
