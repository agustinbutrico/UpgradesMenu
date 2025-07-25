using CardDataLib;
using CardRenderer;
using CardsShared;
using System.Collections;
using System.Linq;
using UnityEngine;
using UpgradesMenu.Menu.Logic;

namespace UpgradesMenu.Menu.UI.CardTreeLoader
{
    internal class Monsters
    {
        internal static IEnumerator LoadMonsterActiveUnlockedCards(
            string menuPath,
            GameObject menuGO,
            string sliceVariant,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var allMonster = new[]
            {
                Subcategory.MonsterHealthBuff, Subcategory.MonsterArmorBuff,
                Subcategory.MonsterShieldBuff, Subcategory.MonsterMoveSpeedBuff,
                Subcategory.MonsterTowerDamageBuff, 
                Subcategory.MonsterSlowDebuff, Subcategory.MonsterGoldDebuff,
                Subcategory.MonsterHasteBuff, Subcategory.MonsterDamageBuff
            };

            float currentY = 0f;

            foreach (var subcategory in allMonster)
            {
                string monsterName = subcategory.ToString();

                var cardsInPanel = CardDataLibAPI.FilterCards(
                    Category.Monster,
                    subcategory,
                    AcquisitionType.AlwaysInRun,
                    true
                );

                if (cardsInPanel.Count == 0)
                    continue;

                string parentPath = $"{menuPath}/{monsterName}";
                Vector2 panelPosition = new Vector2(0, -currentY);

                var panelGO = Panels.CreatePanel(monsterName, menuGO.transform, panelPosition);

                if (panelGO == null)
                    continue;

                var treeRoots = CardTreeBuilder.BuildCardTrees(cardsInPanel.ToList());
                CardRendererAPI.DisplayCardTree(treeRoots, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant);
                CardRendererAPI.ResizeToFitChildren(panelGO);

                yield return null; // Let layout settle

                RectTransform rect = panelGO.GetComponent<RectTransform>();
                float panelHeight = rect.sizeDelta.y;

                currentY += panelHeight;
            }
        }
    }
}
