using BTD_Mod_Helper.Api.Data;
using BTD_Mod_Helper.Api.ModOptions;

namespace SeedChanger
{
    public class Settings : ModSettings
    {
        public static readonly ModSettingHotkey ShowSeedPopup = new(UnityEngine.KeyCode.S)
        {
            description = "If not in freeplay, allow the user to change the seed, else just show the current seed."
        };
    }
}