using HarmonyLib;
using Discord;
using System;
// Originally from Town of Us Rewritten, by Det
namespace TOHEBR.Patches
{
    [HarmonyPatch(typeof(ActivityManager), nameof(ActivityManager.UpdateActivity))]
    public class DiscordRPC
    {
        private static string lobbycode = "";
        private static string region = "";
        public static void Prefix([HarmonyArgument(0)] Activity activity)
        {
            var details = $"TOHE Brasil v{Main.PluginDisplayVersion}";
            activity.Details = details;

            try
            {
                if (activity.State != "No Menu")
                {
                    if (Options.ShowLobbyCode.GetBool())
                    {
                        int maxSize = GameOptionsManager.Instance.currentNormalGameOptions.MaxPlayers;
                        if (GameStates.IsLobby)
                        {
                            lobbycode = GameStartManager.Instance.GameRoomNameCode.text;
                            region = ServerManager.Instance.CurrentRegion.Name;
                            if (region == "North America") region = "NA";
                            if (region == "Europe") region = "EU";
                            if (region == "Asia") region = "AS";
                        }

                        if (lobbycode != "" && region != "")
                        {
                            details = $"TOHE Brasil - {lobbycode} ({region})";
                        }

                        activity.Details = details;
                    }
                    else
                    {
                        details = $"TOHE Brasil v{Main.PluginDisplayVersion}";
                    }
                }
            }

            catch (ArgumentException ex)
            {
                Logger.Error("Error in updating discord rpc", "DiscordPatch");
                Logger.Exception(ex, "DiscordPatch");
                details = $"TOHEBR v{Main.PluginDisplayVersion}";
                activity.Details = details;
            }
        }
    }
}