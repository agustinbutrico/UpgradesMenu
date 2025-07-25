using UpgradesMenu.Utility;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Menu.Style.Card
{
    public static class SliceColor
    {
        internal static string GetCardLockedSlice(StyleType type)
        {
            switch (type)
            {
                case StyleType.Active:
                    return $"UI9Slice{ConfigManager.ActiveCardColor.Value}Filled";
                case StyleType.Passive:
                    return $"UI9Slice{ConfigManager.PassiveCardColor.Value}Filled";
                case StyleType.Monster:
                    return $"UI9Slice{ConfigManager.MonsterCardColor.Value}Filled";
                case StyleType.Banish:
                    return $"UI9Slice{ConfigManager.BanishCardColor.Value}Filled";
                default:
                    return "UI9SliceBrownFilled";
            };
        }

        internal static string GetCardUnlockedSlice(StyleType type)
        {
            switch (type)
            {
                case StyleType.Active:
                    return $"UI9Slice{ConfigManager.ActiveCardColor.Value}";
                case StyleType.Passive:
                    return $"UI9Slice{ConfigManager.PassiveCardColor.Value}";
                case StyleType.Monster:
                    return $"UI9Slice{ConfigManager.MonsterCardColor.Value}";
                case StyleType.Banish:
                    return $"UI9Slice{ConfigManager.BanishCardColor.Value}";
                default:
                    return "UI9SliceBrown";
            };
        }
    }
}
