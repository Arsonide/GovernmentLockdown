﻿using BepInEx;
using HarmonyLib;
using SOD.Common.BepInEx;

namespace GovernmentLockdown;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class GovernmentLockdownPlugin : PluginController<GovernmentLockdownPlugin>
{
    public override void Load()
    {
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        Harmony harmony = new Harmony($"{MyPluginInfo.PLUGIN_GUID}");
        harmony.PatchAll();
        
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
        
        // ClassInjector.RegisterTypeInIl2Cpp<ClassName>();

        // Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} has added custom types!");
    }
}