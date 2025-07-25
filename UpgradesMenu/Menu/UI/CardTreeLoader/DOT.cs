using CardDataLib;
using CardRenderer;
using CardsShared;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace UpgradesMenu.Menu.UI.CardTreeLoader
{
    internal class DOT
    {
        internal static IEnumerator LoadDOTUnlockedCards(
            string menuPath,
            string sliceVariant,
            string panelSliceVariant = null,
            string prefabAspectName = null,
            Color? colorVariant = null,
            Color? textColorVariant = null
        )
        {
            var allDOT = new[]
            {
                Subcategory.DOTBleed, Subcategory.DOTBurn, Subcategory.DOTPoison
            };

            float currentY = 0f;
            float panelSpacing = 10f;

            foreach (var subcategory in allDOT)
            {
                string DOTName = subcategory.ToString();
                var card = CardDataLibAPI.GetCard($"{DOTName}1");
                if (card == null || !card.Unlocked)
                    continue;

                var cardsInPanel = CardDataLibAPI.FilterCards(
                    Category.DOT,
                    subcategory,
                    AcquisitionType.AlwaysInRun | AcquisitionType.StoreUnlocksInRun,
                    unlocked: true
                );

                if (cardsInPanel.Count == 0)
                    continue;

                string parentPath = $"{menuPath}/{DOTName}";
                Vector2 panelPosition = new Vector2(0, -currentY);
                var panelGO = CardRendererAPI.NewPanel(menuPath, DOTName, panelPosition, true, panelSliceVariant);

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
    }
}
