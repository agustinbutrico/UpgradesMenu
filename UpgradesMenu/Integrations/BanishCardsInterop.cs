using System;
using System.Collections.Generic;
using System.Reflection;

namespace UpgradesMenu.Integrations
{
    public static class BanishCardsInterop
    {
        public static List<string> GetBanishedUnlockNames()
        {
            // Get the type from the loaded assemblies
            Type banishedCardsType = Type.GetType("BanishCards.Runtime.BanishedCards, BanishCards");
            if (banishedCardsType == null)
            {
                // BanishCards mod not installed
                return null;
            }

            // Get the static field
            FieldInfo field = banishedCardsType.GetField("BanishedUnlockNames", BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                // Field missing — possible mod update
                UnityEngine.Debug.LogWarning("BanishedUnlockNames field not found in BanishCards.");
                return null;
            }

            // Try to get the value safely
            object value = field.GetValue(null);
            if (value is List<string> list)
                return list;

            UnityEngine.Debug.LogWarning("BanishedUnlockNames field is not a List<string>.");
            return null;
        }
    }
}
