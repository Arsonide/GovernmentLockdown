﻿using BepInEx.Logging;

namespace GovernmentLockdown;

public static class Utilities
{
    public const bool DEBUG_BUILD = true;
    
    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
        // Debug does not appear, presumably because it's for some functionality we don't have. We'll use it to filter based on DEBUG_BUILD instead.
        if (level == LogLevel.Debug)
        {
#pragma warning disable CS0162

            if (DEBUG_BUILD)
            {
                level = LogLevel.Info;
            }
            else
            {
                return;
            }
            
#pragma warning restore CS0162
        }
        
        GovernmentLockdownPlugin.Log.Log(level, message);
    }
}