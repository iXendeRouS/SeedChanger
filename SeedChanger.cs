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

            if (Settings.ShowSeedPopup.JustPressed() && !PopupScreen.instance.IsPopupActive())
            {
                if (InGameData.CurrentGame.IsSandbox || !InGame.instance.IsFreePlay)
                {
                    PromptSeedChange();
                }
                else
                {
                    ShowCurrentSeed();
                }
            }
        }

        private void PromptSeedChange()
        {
            PopupScreen.instance.SafelyQueue(screen =>
                screen.ShowSetNamePopup(
                    "Seed Changer",
                    "Enter new seed: ",
                    (Action<string>)HandleSeedInput,
                    InGame.Bridge.GetFreeplayRoundSeed().ToString()
                )
            );
        }

        private void ShowCurrentSeed()
        {
            int seed = InGame.Bridge.GetFreeplayRoundSeed();

            MelonLogger.Msg($"Current Seed: {seed}");

            PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup(
                "Seed Changer",
                $"Current Seed:",
                null,
                seed.ToString()
            ));
        }

        private void HandleSeedInput(string s)
        {
            if (int.TryParse(s, out int seed))
            {
                if (seed != InGame.Bridge.GetFreeplayRoundSeed())
                    SetSeed(seed);
            }
            else
            {
                int hash = string.IsNullOrEmpty(s) ? 0 : s.GetHashCode();
                SetSeed(hash);
            }
        }

        private void SetSeed(int seed)
        {
            MelonLogger.Msg($"Setting seed to {seed}");

            MapSaveDataModel? saveModel = InGame.instance.CreateCurrentMapSave(
                InGame.instance.GetSimulation().GetCurrentRound() - 1,
                InGame.instance.MapDataSaveId
            );

            saveModel.freeplayRoundSeed = seed;

            InGame.Bridge.ExecuteContinueFromCheckpoint(
                InGame.Bridge.MyPlayerNumber,
                new KonFuze(),
                ref saveModel,
                true,
                false
            );

            if (!InGameData.CurrentGame.IsSandbox)
            {
                Game.Player.Data.SetSavedMap(saveModel.savedMapsId, saveModel);
            }

            PopupScreen.instance.SafelyQueue(screen => screen.ShowOkPopup(
                $"Set seed to {seed}"
            ));
        }
    }
}