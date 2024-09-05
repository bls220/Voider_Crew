using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace Voider_Crew;

[BepInPlugin(ThisAssembly.PluginGUID, ThisAssembly.PluginTitle, ThisAssembly.AssemblyFileVersion)]
[BepInProcess("Void Crew.exe")]
[BepInDependency(VoidManager.MyPluginInfo.PLUGIN_GUID)]
public partial class Plugin : BaseUnityPlugin
{
    public static ManualLogSource? StaticLogger { get; private set; }
    public static Plugin Instance { get; private set; } = null!;

    private static readonly Harmony _harmony = new(ThisAssembly.PluginGUID);

    protected static int NewMaxPlayers { get; private set; } = 8;

    private int CurrentMaxPlayer { get; set; } = 4;

    protected Plugin()
    {
        Instance = this;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
    private void Awake()
    {
        StaticLogger = Logger;
        Logger.LogDebug($"Plugin {ThisAssembly.PluginGUID} is loading!");
        try
        {
            _harmony.PatchAll();
        }
        catch (HarmonyException ex)
        {
            Logger.LogError(ex.Message);
            Logger.LogError($"Plugin {ThisAssembly.PluginGUID} failed to patch!");
            return;
        }
        Logger.LogInfo($"Plugin {ThisAssembly.PluginGUID} is loaded!");
    }
}