using UpgradesMenu.Utility;
using static UpgradesMenu.Menu.Style.StyleManager;

namespace UpgradesMenu.Menu.Style.Panel
{
    internal class SliceColor
    {
        internal static string GetPanelSlice(StyleType type)
        {
            switch (type)
            {
                case StyleType.Active:
                    return $"LargeUI9Slice{ConfigManager.ActivePanelColor.Value}";
                case StyleType.Passive:
                    return $"LargeUI9Slice{ConfigManager.PassivePanelColor.Value}";
                case StyleType.Monster:
                    return $"LargeUI9Slice{ConfigManager.MonsterPanelColor.Value}";
                default:
                    return "LargeUI9SliceOrange";
            };
        }
    }
}
