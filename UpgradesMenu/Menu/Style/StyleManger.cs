using UnityEngine;

namespace UpgradesMenu.Menu.Style
{
    internal class StyleManager
    {
        public enum StyleType
        {
            Active,
            Passive,
            Monster,
            Banish
        }

        internal struct CardVisualStyle
        {
            public string SliceLocked;
            public string SliceUnlocked;
            public Color IconColor;
            public Color TextColor;
        }

        internal struct PanelVisualStyle
        {
            public string Slice;
        }

        internal static CardVisualStyle GetCardStyle(StyleType type)
        {
            return new CardVisualStyle
            {
                SliceLocked = Card.SliceColor.GetCardLockedSlice(type),
                SliceUnlocked = Card.SliceColor.GetCardUnlockedSlice(type),
                IconColor = Card.IconColor.GetCardIconColor(type),
                TextColor = Card.TextColor.GetCardTextColor(type)
            };
        }

        internal static PanelVisualStyle GetPanelStyle(StyleType type)
        {
            return new PanelVisualStyle
            {
                Slice = Panel.SliceColor.GetPanelSlice(type)
            };
        }
    }
}
