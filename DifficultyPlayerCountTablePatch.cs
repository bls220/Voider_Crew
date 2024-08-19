using System;

using Gameplay.MissionDifficulty;

using HarmonyLib;

namespace Voider_Crew;

public partial class Plugin
{
    [HarmonyPatch(typeof(DifficultyPlayerCountTable))]
    private static class DifficultyPlayerCountTablePatch
    {
        [HarmonyFinalizer, HarmonyPatch(nameof(DifficultyPlayerCountTable.GetConfig))]
        private static Exception? GetConfig_Finalizer(int playerCount, Exception? __exception, DifficultyPlayerCountTable __instance, ref DifficultyConfig __result)
        {
            // Config found or other bug -- passthrough
            if (__exception is null || playerCount <= 4)
            {
                Plugin.StaticLogger?.LogDebug($"Found Difficulty config for {playerCount:D} players. Config = {__result}");
                return __exception;
            }

            // No Modded configs were found, try to get default max config
            Plugin.StaticLogger?.LogDebug($"Did NOT find Difficulty config for {playerCount:D} players. Config = {__result}");
            try
            {
                __result = __instance.GetConfig(4);
                return null;
            }
            catch (Exception)
            {
                // We failed, return original exception.
                return __exception;
            }
        }
    }
}
