using BepInEx;
using BepInEx.Configuration;
using SOD.Common.BepInEx;

namespace GovernmentLockdown;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class GovernmentLockdownPlugin : PluginController<GovernmentLockdownPlugin>
{
    public static ConfigEntry<bool> RemoveUnsecureSecurityApp;
    public static ConfigEntry<bool> RemoveUnsecureSurveillanceApp;

    public override void Load()
    {
        base.Load();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        Harmony.PatchAll();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");

        BindConfig();
    }

    private void BindConfig()
    {
        RemoveUnsecureSecurityApp = Config.Bind("Options", "Remove Unsecure Security App", true,
                                                new ConfigDescription("Removes the security app from city hall offices if they aren't secure."));
        
        RemoveUnsecureSurveillanceApp = Config.Bind("Options", "Remove Unsecure Surveillance App", true,
                                                    new ConfigDescription("Removes the surveillance app from city hall offices if they aren't secure."));
    }
}
