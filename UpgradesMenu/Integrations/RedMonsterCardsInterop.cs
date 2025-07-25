using System;
using System.Linq;

namespace UpgradesMenu.Integrations
{
    public static class RedMonsterCardsInterop
    {
        public static bool IsModPresent()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Any(a => a.GetName().Name == "RustyDios.RedMonsterCards");
        }
    }
}
