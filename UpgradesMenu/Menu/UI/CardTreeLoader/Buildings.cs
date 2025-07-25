using CardDataLib;
using CardsShared;
using CardRenderer;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace UpgradesMenu.Menu.UI.CardTreeLoader
{
    internal class Buildings
    {
        internal static IEnumerator LoadBuildingUnlockedCards(
            string menuPath,
            string sliceVariant,
            string panelSliceVariant = null,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var allBuildings = new[]
            {
                Subcategory.ManaSiphon, Subcategory.Mine, Subcategory.ManaBank, Subcategory.HauntedHouse, Subcategory.University
            };

            float currentY = 0f;
            float panelSpacing = 10f;

            foreach (var subcategory in allBuildings)
            {
                string buildingName = subcategory.ToString();
                var card = CardDataLibAPI.GetCard(buildingName);
                if (card == null || !card.Unlocked)
                    continue;

                var cardsInPanel = CardDataLibAPI.FilterCards(
                    Category.Building,
                    subcategory,
                    AcquisitionType.AlwaysInRun | AcquisitionType.StoreUnlocksInRun,
                    unlocked: true
                );

                if (cardsInPanel.Count == 0)
                    continue;

                string parentPath = $"{menuPath}/{buildingName}";
                Vector2 panelPosition = new Vector2(0, -currentY);
                var panelGO = CardRendererAPI.NewPanel(menuPath, buildingName, panelPosition, true, panelSliceVariant);

                if (panelGO == null)
                    continue;

                var treeRoots = CardTreeBuilder.BuildCardTrees(cardsInPanel.ToList());
                CardRendererAPI.DisplayCardTree(treeRoots, parentPath, sliceVariant, prefabAspectName, colorVariant, textColorVariant, true);
                CardRendererAPI.ResizeToFitChildren(panelGO, true);

                yield return null; // Let layout settle

                RectTransform rect = panelGO.GetComponent<RectTransform>();
                float panelHeight = rect.sizeDelta.y;

                currentY += panelHeight + panelSpacing;
            }
        }
    }
}
