using HarmonyLib;
using Steamworks;

namespace Voider_Crew;

public partial class Plugin
{
    [HarmonyPatch(typeof(SteamMatchmaking))]
    private static class SteamMatchmakingPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(SteamMatchmaking.CreateLobby))]
        private static void CreateLobby_Prefix(ELobbyType eLobbyType, ref int cMaxMembers)
        {
            Plugin.StaticLogger?.LogDebug($"Intercepted CreateLobby with LobbyType: {eLobbyType}, MaxMembers: {cMaxMembers}. Changing to {Plugin.NewMaxPlayers}");
            cMaxMembers = Plugin.NewMaxPlayers;
        }
    }
}
