using Gameplay.GalaxyMap;
using HarmonyLib;

namespace Voider_Crew;

public partial class Plugin
{
    [HarmonyPatch(typeof(QuestInfoPopUp))]
    private static class QuestInfoPopUpPatch
    {
        [HarmonyPostfix, HarmonyPatch("OnPlayerCountUpdated")]
        private static void OnPlayerCountUpdated_Postfix(ref UnityEngine.UI.Button ___startButton)
        {
            ___startButton.interactable = true;
        }
    }
}
