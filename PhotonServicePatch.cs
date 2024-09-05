using HarmonyLib;

using Photon.Pun;

namespace Voider_Crew;

public partial class Plugin
{
#if DEBUG
    [HarmonyPatch]
    private static class PhotonServicePatch
    {

        [HarmonyPrefix, HarmonyPatch(typeof(PhotonService), nameof(PhotonService.SetCurrentRoomPlayerLimit), typeof(int))]
        private static bool SetCurrentRoomPlayerLimitPatch(int limit)
        {
            Plugin.StaticLogger?.LogDebug($"PhotonService: Changed Player limit: {limit}");
            return PhotonNetwork.IsMasterClient;
        }
    }
#endif
}
