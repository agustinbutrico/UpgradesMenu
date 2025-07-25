using System.Collections;
using UnityEngine;
using UpgradesMenu.Menu.Logic;
using UpgradesMenu.Menu.Style;
using UpgradesMenu.Menu.UI.CardTreeLoader;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Menu.UI.PanelsLoader
{
    internal class PassiveCards
    {
        internal static IEnumerator SetupPassiveCards(GameObject slidingScreenGO, string menuPath)
        {
            string prefAspectName = "DefaultHorizontalCard";
            var cardStyle = GetCardStyle(StyleType.Passive);
            var panelStyle = GetPanelStyle(StyleType.Passive);
            
            // Passive Panel
            var passiveCardsGO = Panels.CreatePanel("PassiveCards", slidingScreenGO.transform);

            // Buff Passive Cards
            var buffGO = Panels.CreatePanel("BuffCards", passiveCardsGO.transform);
            yield return Buffs.LoadBuffPassiveCards($"{menuPath}/BuffCards", cardStyle.SliceLocked, panelStyle.Slice, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);
            Vector2 buffSize = Vector2.zero;
            yield return Panels.GetPanelDimensions(menuPath, "BuffCards", s => buffSize = s);

            // Tower Passive Cards
            var towerGO = Panels.CreatePanel("TowerCards", passiveCardsGO.transform);
            yield return Towers.LoadTowerPassiveCards($"{menuPath}/TowerCards", cardStyle.SliceLocked, panelStyle.Slice, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);
            Vector2 towerSize = Vector2.zero;
            yield return Panels.GetPanelDimensions(menuPath, "TowerCards", s => towerSize = s);

            // Shift full passive panel
            float buffYShift = towerSize.y > 0f ? buffSize.y + towerSize.y + 50f : buffSize.y + 40f;
            float towerYShift = towerSize.y > 0f ? towerSize.y + 40f : 0f;
            buffGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, buffYShift);
            towerGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, towerYShift);
        }
    }
}
