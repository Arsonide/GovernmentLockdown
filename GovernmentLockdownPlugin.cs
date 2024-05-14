using BepInEx;
using SOD.Common.BepInEx;

namespace GovernmentLockdown;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class GovernmentLockdownPlugin : PluginController<GovernmentLockdownPlugin>
{
    public override void Load()
    {
        base.Load();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        Harmony.PatchAll();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
    }
}
