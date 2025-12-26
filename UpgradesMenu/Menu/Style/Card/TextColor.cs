using UnityEngine;
using UpgradesMenu.Utility;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Menu.Style.Card
{
    internal class TextColor
    {
        internal static Color GetCardTextColor(StyleType type)
        {
            string colorKey;

            switch (type)
            {
                case StyleType.Active:
                    colorKey = ConfigManager.ActiveCardColor.Value;
                    break;
                case StyleType.Passive:
                    colorKey = ConfigManager.PassiveCardColor.Value;
                    break;
                case StyleType.Monster:
                    colorKey = ConfigManager.MonsterCardColor.Value;
                    break;
                case StyleType.Banish:
                    colorKey = ConfigManager.BanishCardColor.Value;
                    break;
                default:
                    colorKey = "orange";
                    break;
            }

            return ColorsHelper.GetContrastByName(colorKey);
        }
    }
}
