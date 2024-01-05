using BepInEx;
using SOD.Common.BepInEx;

namespace GovernmentLockdown;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(PrintBugfix.Plugin.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
public class GovernmentLockdownPlugin : PluginController<GovernmentLockdownPlugin>
{
    public override void Load()
    {
        base.Load();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        Harmony.PatchAll();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
        
        // ClassInjector.RegisterTypeInIl2Cpp<ClassName>();
        // Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} has added custom types!");
        
        PrintBugfix.Plugin.RegisterAffectedPreset("BirthCertificate");
    }

    public override bool Unload()
    {
        PrintBugfix.Plugin.UnregisterAffectedPreset("BirthCertificate");
        return base.Unload();
    }
}
