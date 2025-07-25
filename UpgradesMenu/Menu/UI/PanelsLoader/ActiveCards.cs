using System.Collections;
using UnityEngine;
using UpgradesMenu.Menu.Logic;
using UpgradesMenu.Menu.Style;
using UpgradesMenu.Menu.UI.CardTreeLoader;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Menu.UI.PanelsLoader
{
    internal class ActiveCards
    {
        internal static IEnumerator SetupActiveCards(GameObject slidingScreenGO, string menuPath)
        {
            string prefAspectName = "DefaultHorizontalCard";
            var cardStyle = GetCardStyle(StyleType.Active);
            var panelStyle = GetPanelStyle(StyleType.Active);

            // Active Panel
            var activeCardsGO = Panels.CreatePanel("ActiveCards", slidingScreenGO.transform);

            // Tower Cards
            var towerGO = Panels.CreatePanel("TowerCards", activeCardsGO.transform);
            yield return Towers.LoadTowerActiveUnlockedCards($"{menuPath}/TowerCards", cardStyle.SliceLocked, panelStyle.Slice, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);

            // DOT Cards
            var DOTGO = Panels.CreatePanel("DOTCards", activeCardsGO.transform, new Vector2(-140f, 0f));
            yield return DOT.LoadDOTUnlockedCards($"{menuPath}/DOTCards", cardStyle.SliceLocked, panelStyle.Slice, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);
            Vector2 DOTSize = Vector2.zero;
            yield return Panels.GetPanelDimensions(menuPath, "DOTCards", s => DOTSize = s);

            // Building Cards
            float buildingYShift = DOTSize.y > 0f ? DOTSize.y + 10f : 0f;
            var buildingGO = Panels.CreatePanel("BuildingCards", activeCardsGO.transform, new Vector2(-140f, -buildingYShift));
            yield return Buildings.LoadBuildingUnlockedCards($"{menuPath}/BuildingCards", cardStyle.SliceLocked, panelStyle.Slice, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);

            // Buff Cards
            var buffGO = Panels.CreatePanel("BuffCards", activeCardsGO.transform);
            yield return Buffs.LoadBuffActiveUnlockedCards($"{menuPath}/BuffCards", cardStyle.SliceLocked, panelStyle.Slice, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);
            Vector2 buffActiveSize = Vector2.zero;
            yield return Panels.GetPanelDimensions(menuPath, "BuffCards", s => buffActiveSize = s);

            // Shift full Buff panel
            buffGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140f, buffActiveSize.y + 40f);
        }
    }
}
