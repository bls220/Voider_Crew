using VoidManager.MPModChecks;

namespace Voider_Crew;

public class VoidManagerPlugin : VoidManager.VoidPlugin
{
    public override MultiplayerType MPType => MultiplayerType.All;

    public override string Author => ThisAssembly.PluginAuthors;

    public override string Description => ThisAssembly.PluginDescription;
}