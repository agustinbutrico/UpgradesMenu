using CardDataLib;
using CardsShared;
using CardRenderer;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace UpgradesMenu.Menu.UI.CardTreeLoader
{
    internal class Buffs
    {
        internal static IEnumerator LoadBuffActiveUnlockedCards(
            string menuPath,
            string sliceVariant,
            string panelSliceVariant = null,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var allBuff = new[]
            {
                Subcategory.TreeBuff, Subcategory.CritsBuff, Subcategory.GoldBuff,
                Subcategory.ManaBuff, Subcategory.TowerBuff
            };

            float currentY = 0f;
            float panelSpacing = 10f;

            foreach (var subcategory in allBuff)
            {
                string buffName = subcategory.ToString();

                var cardsInPanel = CardDataLibAPI.FilterCards(
                    Category.Buff,
                    subcategory,
                    AcquisitionType.AlwaysInRun | AcquisitionType.StoreUnlocksInRun,
                    unlocked: true
                );

                if (cardsInPanel.Count == 0)
                    continue;

                string parentPath = $"{menuPath}/{buffName}";
                Vector2 panelPosition = new Vector2(0, -currentY);
                var panelGO = CardRendererAPI.NewPanel(menuPath, buffName, panelPosition, true, panelSliceVariant);

                if (panelGO == null)
                    continue;

                var treeRoots = CardTreeBuilder.BuildCardTrees(cardsInPanel.ToList());
                CardRendererAPI.DisplayCardTree(treeRoots, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant, true, depth0YSpacing: 0f);
                CardRendererAPI.ResizeToFitChildren(panelGO, true);

                yield return null; // Let layout settle

                RectTransform rect = panelGO.GetComponent<RectTransform>();
                float panelHeight = rect.sizeDelta.y;

                currentY += panelHeight + panelSpacing;
            }
        }

        internal static IEnumerator LoadBuffPassiveCards(
            string menuPath,
            string sliceVariant,
            string panelSliceVariant = null,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var buffColumns = new[]
            {
                new[] { Subcategory.GoldBuff, Subcategory.TowerBuff },
                new[] { Subcategory.DrawBuff, Subcategory.ManaBuff }
            };

            float currentX = 0f;
            float columnSpacing = 10f;

            foreach (var column in buffColumns)
            {
                float currentY = 0f;
                float rowSpacing = 10f;
                float maxWidthInColumn = 0f;
                bool columnHasVisiblePanels = false;

                foreach (var subcategory in column)
                {
                    string buffName = subcategory.ToString();

                    var cardsInPanel = CardDataLibAPI.FilterCards(
                        Category.Buff,
                        subcategory,
                        AcquisitionType.StoreAppliesPermanent
                    );

                    if (cardsInPanel.Count == 0)
                        continue;

                    columnHasVisiblePanels = true;

                    string parentPath = $"{menuPath}/{buffName}";
                    Vector2 panelPosition = new Vector2(currentX, -currentY);
                    var panelGO = CardRendererAPI.NewPanel(menuPath, buffName, panelPosition, spriteName: panelSliceVariant);

                    if (panelGO == null)
                        continue;

                    var treeRoots = CardTreeBuilder.BuildCardTrees(cardsInPanel.ToList());
                    CardRendererAPI.DisplayCardTree(treeRoots, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant, depth0YSpacing: 0f);
                    CardRendererAPI.ResizeToFitChildren(panelGO);

                    yield return null; // Let layout settle

                    RectTransform rect = panelGO.GetComponent<RectTransform>();
                    float panelHeight = rect.sizeDelta.y;
                    float panelWidth = rect.sizeDelta.x;

                    currentY += panelHeight + rowSpacing;

                    if (panelWidth > maxWidthInColumn)
                        maxWidthInColumn = panelWidth;
                }

                if (columnHasVisiblePanels)
                {
                    currentX += maxWidthInColumn + columnSpacing;
                }
            }
        }
    }
}
