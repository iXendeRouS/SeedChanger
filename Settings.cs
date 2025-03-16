using BTD_Mod_Helper.Api.Data;
using BTD_Mod_Helper.Api.ModOptions;

namespace SeedChanger
{
    public class Settings : ModSettings
    {
        public static readonly ModSettingBool EnableMod = new(true);

        public static readonly ModSettingHotkey PromptSeedChange = new(UnityEngine.KeyCode.S)
        {
            description = "When pressed, show the popup to change seeds. Only works at the start of a game or in sandbox."
        };

        public static readonly ModSettingHotkey ShowCurrentSeed = new(UnityEngine.KeyCode.L)
        {
            description = "When pressed, show a popup with and log the current freeplay seed."
        };
    }
}