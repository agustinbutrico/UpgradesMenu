using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace UpgradesMenu.Utility
{
    internal static class ConfigManager
    {
        internal static ConfigEntry<string> ActivePanelColor;
        internal static ConfigEntry<string> ActiveCardColor;
        internal static ConfigEntry<string> PassivePanelColor;
        internal static ConfigEntry<string> PassiveCardColor;
        internal static ConfigEntry<string> MonsterPanelColor;
        internal static ConfigEntry<string> MonsterCardColor;

        internal static ConfigEntry<bool> OverrideRedMonsterCards;
        internal static ConfigEntry<string> BanishCardColor;

        internal static ConfigEntry<KeyCode> ToggleKey;
        internal static ConfigEntry<float> DefaultZoom;
        internal static ConfigEntry<float> ZoomSpeed;
        internal static ConfigEntry<float> MinZoom;
        internal static ConfigEntry<float> MaxZoom;

        internal static void Initialize(BaseUnityPlugin plugin)
        {
            string availableColors = "Can be: White, Grey, Black, Blue, Brown, Red, Green, Purple, DeepBlue, DeepBrown, DeepRed, DeepGreen, DeepPurple";

            ActivePanelColor = plugin.Config.Bind("Appearance", "ActivePanelColor", "Brown", availableColors);
            ActiveCardColor = plugin.Config.Bind("Appearance", "ActiveCardColor", "Brown", availableColors);

            PassivePanelColor = plugin.Config.Bind("Appearance", "PassivePanelColor", "Blue", availableColors);
            PassiveCardColor = plugin.Config.Bind("Appearance", "PassiveCardColor", "Blue", availableColors);

            MonsterPanelColor = plugin.Config.Bind("Appearance", "MonsterPanelColor", "Blue", availableColors);
            MonsterCardColor = plugin.Config.Bind("Appearance", "MonsterCardColor", "Blue", availableColors);

            OverrideRedMonsterCards = plugin.Config.Bind("Compatibility", "OverrideRedMonsterCards", false, "Override monster card colors if RedMonsterCards mod is installed.");

            BanishCardColor = plugin.Config.Bind("Compatibility", "BanishCardColor", "Grey", availableColors);

            ToggleKey = plugin.Config.Bind("Input", "ToggleKey", KeyCode.Tab, "Toggles the UpgradesMenu");

            DefaultZoom = plugin.Config.Bind("Zoom", "DefaultZoom", 0.5f, "Zoom on first open");
            ZoomSpeed = plugin.Config.Bind("Zoom", "ZoomSpeed", 10f, "Zoom speed for the sliding screen");
            MinZoom = plugin.Config.Bind("Zoom", "MinZoom", 0.25f, "Minimum zoom level");
            MaxZoom = plugin.Config.Bind("Zoom", "MaxZoom", 2f, "Maximum zoom level");
        }
    }
}
