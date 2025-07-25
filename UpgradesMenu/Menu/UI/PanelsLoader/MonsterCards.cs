using CardRenderer;
using System.Collections;
using UnityEngine;
using UpgradesMenu.Menu.Logic;
using UpgradesMenu.Menu.UI.CardTreeLoader;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Menu.UI.PanelsLoader
{
    internal class MonsterCards
    {
        internal static IEnumerator SetupMonsterCards(GameObject slidingScreenGO, string slidingScreenPath)
        {
            string prefAspectName = "DefaultHorizontalCard";
            var cardStyle = GetCardStyle(StyleType.Monster);
            var panelStyle = GetPanelStyle(StyleType.Monster);

            string monsterMenuPath = $"{slidingScreenPath}/MonsterCards";

            // Monster Panel
            var monsterCardsGO = Panels.CreatePanel("MonsterCards", slidingScreenGO.transform);

            Vector2 towerActiveSize = Vector2.zero;
            yield return Panels.GetPanelDimensions($"{slidingScreenPath}/ActiveCards", "TowerCards", s => towerActiveSize = s);

            // Monster Cards
            float monsterYShift = towerActiveSize.y > 0f ? towerActiveSize.y + 50f : 0f;
            var monsterGO = CardRendererAPI.NewPanel(monsterMenuPath, "MonsterCards", new Vector2(0f, -monsterYShift), spriteName: panelStyle.Slice);
            yield return Monsters.LoadMonsterActiveUnlockedCards($"{monsterMenuPath}/MonsterCards", monsterGO, cardStyle.SliceLocked, prefAspectName, cardStyle.IconColor, cardStyle.TextColor);
            CardRendererAPI.ResizeToFitChildren(monsterGO);
            var monsterRect = monsterGO.GetComponent<RectTransform>();
            var monsterSize = monsterRect.sizeDelta;
            monsterSize.x -= 40f;
            monsterSize.y -= 40f;
            monsterRect.sizeDelta = monsterSize;
        }
    }
}
