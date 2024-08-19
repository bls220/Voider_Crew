using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using HarmonyLib;

using Photon.Pun;

using UI.Settings;

using UnityEngine.UIElements;

namespace Voider_Crew;

public partial class Plugin
{

    [HarmonyPatch(typeof(MatchmakingRoomSetup), MethodType.Constructor, typeof(VisualElement))]
    private static class MatchmakingRoomSetupPatch
    {

        private static IntSetting Patch()
        {
            var setting = new IntSetting(Plugin.NewMaxPlayers, 1, Plugin.NewMaxPlayers);
            setting.OnValueChanged = (Action<int>)Delegate.Combine(setting.OnValueChanged, new Action<int>((newVal) => OnPlayerLimitChanged(setting, newVal)));
            return setting;
        }

        private static void OnPlayerLimitChanged(IntSetting setting, int newValue)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                setting.SetValue(Plugin.Instance.CurrentMaxPlayer, skipBroadcast: true);
            }
            Plugin.StaticLogger?.LogDebug($"Player limit changed to: {newValue}, setting = {setting.Value}");
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Constructor_Transpiler(IEnumerable<CodeInstruction> codeInstructionsEnumerable)
        {
            var codeInstructions = codeInstructionsEnumerable.ToList();
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                CodeInstruction current = codeInstructions[i];
                if (current.opcode == OpCodes.Stfld && current.operand == (object)AccessTools.Field(typeof(MatchmakingRoomSetup), "playerLimit"))
                {
                    codeInstructions.Insert(i, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MatchmakingRoomSetupPatch), nameof(Patch))));
                    codeInstructions.Insert(i, new CodeInstruction(OpCodes.Pop));
                    break;
                }
            }
            return codeInstructions;
        }

        [HarmonyPostfix]
        private static void Constructor_Postfix(VisualElement root)
        {
            // Get the player setting slider
            var playerSlider = root.Q<SliderIntSettingEntryVE?>("PlayerSlider", (string?)null, (string?)null);
            if (playerSlider is null)
            {
                Plugin.StaticLogger?.LogError("Unable to find the PlayerSlider");
                return;
            }

            // Get the slider element
            if (AccessTools.Field(typeof(SliderIntSettingEntryVE), "slider").GetValue(playerSlider) is not SliderInt playerLimitInput)
            {
                Plugin.StaticLogger?.LogError("Unable to find the PlayerSlider.slider");
                return;
            }
            // Set the HighValue to our Max Players
            playerLimitInput.highValue = Plugin.NewMaxPlayers;
            playerLimitInput.RegisterValueChangedCallback((evt) =>
            {
                Plugin.StaticLogger?.LogDebug($"Player limit slider changed to {evt.newValue}");
                Plugin.Instance.CurrentMaxPlayer = evt.newValue;
            });
        }
    }
}
