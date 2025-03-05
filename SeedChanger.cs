using MelonLoader;
using BTD_Mod_Helper;
using SeedChanger;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppSystem;

[assembly: MelonInfo(typeof(SeedChanger.SeedChanger), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace SeedChanger
{
    public class SeedChanger : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            ModHelper.Msg<SeedChanger>("SeedChanger loaded!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (InGame.instance == null) return;

            if (Settings.LogCurrentSeed.JustPressed())
            {
                MelonLogger.Msg($"Current Seed: {InGame.instance.bridge.GetFreeplayRoundSeed()}");
            }
        }

        public override void OnMatchStart()
        {
            base.OnMatchStart();

            HandleNewGame();
        }

        public override void OnRestart()
        {
            base.OnRestart();

            if (!Settings.PromptSetSeedOnRestart) return;

            HandleNewGame();
        }

        private void HandleNewGame()
        {
            if (Settings.seed < 0) return;

            if (InGameData.CurrentGame.IsSandbox && Settings.autoSetSeedInSandbox)
            {
                SetSeed(Settings.seed);
                return;
            }

            if (InGame.instance.GetSimulation().roundTime.elapsed == 0)
            {
                PopupScreen.instance.SafelyQueue(screen =>
                    screen.ShowSetValuePopup("Seed Changer", "Enter new seed:", (Action<int>)((int s) => SetSeed(s)), Settings.seed));
            }
        }

        private void SetSeed(int seed)
        {
            MelonLogger.Msg($"Setting seed to {seed}");

            MapSaveDataModel? saveModel = InGame.instance.CreateCurrentMapSave(InGame.instance.GetSimulation().GetCurrentRound() - 1, InGame.instance.MapDataSaveId);

            saveModel.freeplayRoundSeed = seed;

            InGame.Bridge.ExecuteContinueFromCheckpoint(InGame.Bridge.MyPlayerNumber, new KonFuze(), ref saveModel, true, false);

            if (!InGameData.CurrentGame.IsSandbox)
            {
                Game.Player.Data.SetSavedMap(saveModel.savedMapsId, saveModel);
            }
        }
    }
}
