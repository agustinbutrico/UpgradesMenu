using CardDataLib;
using CardsShared;
using CardRenderer;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace UpgradesMenu.Menu.UI.CardTreeLoader
{
    internal class Towers
    {
        internal static IEnumerator LoadTowerActiveUnlockedCards(
            string menuPath,
            string sliceVariant,
            string panelSliceVariant = null,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var towerColumns = new[]
            {
                new[] { Subcategory.Ballista, Subcategory.Mortar, Subcategory.TeslaCoil },
                new[] { Subcategory.FrostKeep, Subcategory.FlameThrower, Subcategory.PoisonSprayer },
                new[] { Subcategory.Shredder, Subcategory.Encampment, Subcategory.Lookout },
                new[] { Subcategory.VampireLair, Subcategory.Cannon, Subcategory.Monument },
                new[] { Subcategory.Radar, Subcategory.Obelisk, Subcategory.ParticleCannon }
            };

            float currentX = 0f;
            float columnSpacing = 10f;

            foreach (var column in towerColumns)
            {
                float currentY = 0f;
                float rowSpacing = 10f;
                float maxWidthInColumn = 0f;
                bool columnHasVisiblePanels = false;

                foreach (var subcategory in column)
                {
                    string towerName = subcategory.ToString();
                    var card = CardDataLibAPI.GetCard(towerName, towerName);
                    if (card == null || !card.Unlocked)
                        continue;

                    columnHasVisiblePanels = true;

                    var cardsInPanel = CardDataLibAPI.FilterCards(
                        Category.Tower,
                        subcategory,
                        AcquisitionType.AlwaysInRun | AcquisitionType.StoreUnlocksInRun,
                        unlocked: true
                    );

                    string parentPath = $"{menuPath}/{towerName}";

                    Vector2 panelPosition = new Vector2(currentX, -currentY);
                    var panelGO = CardRendererAPI.NewPanel(menuPath, towerName, panelPosition, spriteName: panelSliceVariant);

                    if (panelGO == null)
                        continue;

                    var treeRoots = CardTreeBuilder.BuildCardTrees(cardsInPanel.ToList());
                    CardRendererAPI.DisplayCardTree(treeRoots, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant);
                    CardRendererAPI.ResizeToFitChildren(panelGO);

                    yield return null; // let layout settle

                    RectTransform rect = panelGO.GetComponent<RectTransform>();
                    float panelWidth = rect.sizeDelta.x;
                    float panelHeight = rect.sizeDelta.y;

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

        internal static IEnumerator LoadTowerPassiveCards(
            string menuPath,
            string sliceVariant,
            string panelSliceVariant = null,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var towerColumns = new[]
            {
                new[] { Subcategory.Ballista, Subcategory.Mortar, Subcategory.TeslaCoil },
                new[] { Subcategory.FrostKeep, Subcategory.FlameThrower, Subcategory.PoisonSprayer },
                new[] { Subcategory.Shredder, Subcategory.Encampment, Subcategory.Lookout },
                new[] { Subcategory.VampireLair, Subcategory.Cannon, Subcategory.Monument },
                new[] { Subcategory.Radar, Subcategory.Obelisk, Subcategory.ParticleCannon }
            };

            float currentX = 0f;
            float columnSpacing = 10f;

            foreach (var column in towerColumns)
            {
                float currentY = 0f;
                float rowSpacing = 10f;
                float maxWidthInColumn = 0f;
                bool columnHasVisiblePanels = false;

                foreach (var subcategory in column)
                {
                    string towerName = subcategory.ToString();
                    var card = CardDataLibAPI.GetCard(towerName, towerName);
                    if (card == null || !card.Unlocked)
                        continue;

                    var cardsInPanel = CardDataLibAPI.FilterCards(
                        Category.Tower,
                        subcategory,
                        AcquisitionType.StoreAppliesPermanent
                    );

                    if (cardsInPanel.Count == 0)
                        continue;

                    columnHasVisiblePanels = true;

                    string parentPath = $"{menuPath}/{towerName}";
                    Vector2 panelPosition = new Vector2(currentX, -currentY);
                    var panelGO = CardRendererAPI.NewPanel(menuPath, towerName, panelPosition, spriteName: panelSliceVariant);

                    if (panelGO == null)
                        continue;

                    var treeRoots = CardTreeBuilder.BuildCardTrees(cardsInPanel.ToList());
                    CardRendererAPI.DisplayCardTreeVertical(treeRoots, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant);
                    CardRendererAPI.ResizeToFitChildrenVertical(panelGO);

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
