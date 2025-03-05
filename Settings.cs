using BTD_Mod_Helper.Api.Data;
using BTD_Mod_Helper.Api.ModOptions;

namespace SeedChanger
{
    public class Settings : ModSettings
    {
        public static readonly ModSettingBool PromptSetSeedOnRestart = new(false)
        {
            description = "Set seeds do persist after a restart so only enable this if you want to change seeds multiple times."
        };

        public static readonly ModSettingInt seed = new ModSettingInt(0)
        {
            description = "Used for autoSetSeedInSandbox and the default value when prompted to change seed. Set to any negative value to disable the mod.",
        };

        public static readonly ModSettingBool autoSetSeedInSandbox = new(true)
        {
            description = "Automatically set the seed in sandbox"
        };

        public static readonly ModSettingHotkey LogCurrentSeed = new(UnityEngine.KeyCode.S)
        {
            description = "Output the current seed when pressed ingame."
        };
    }
}