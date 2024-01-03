using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DesktopApp), "UpdateIcons")]
public class MunicipalDesktopFilterPatch
{
    private static List<CruncherAppPreset> ReversedApps = new List<CruncherAppPreset>();
    
    [HarmonyPrefix]
    private static bool Prefix(DesktopApp __instance)
    {
        MunicipalComputerType computerType = Utilities.GetMunicipalComputerType(__instance);
        
        if (computerType == MunicipalComputerType.NotMunicipal)
        {
            return true;
        }

        ReversedApps.Clear();
        
        List<CruncherAppPreset> additionalApps = __instance?.controller?.ic?.interactable?.preset?.additionalApps;

        if (additionalApps == null)
        {
            return true;
        }
        
        for (int i = additionalApps.Count - 1; i >= 0; --i)
        {
            CruncherAppPreset preset = additionalApps[i];
            ReversedApps.Add(preset);
            
            switch (preset.presetName)
            {
                case "Surveillance":
                case "Security":
                    if (computerType != MunicipalComputerType.EnforcersSecure)
                    {
                        additionalApps.RemoveAt(i);
                    }

                    break;
                case "GovDatabase":
                    switch (computerType)
                    {
                        case MunicipalComputerType.EnforcersSecure:
                        case MunicipalComputerType.HospitalSecure:
                            // Do nothing, keep it in this case. Hospital will be converted to Medical.
                            break;
                        default:
                            additionalApps.RemoveAt(i);
                            break;
                    }

                    break;
            }
        }

        return true;
    }
    
    [HarmonyPostfix]
    private static void Postfix(DesktopApp __instance)
    {
        MunicipalComputerType computerType = Utilities.GetMunicipalComputerType(__instance);
        
        if (computerType == MunicipalComputerType.NotMunicipal)
        {
            return;
        }
        
        List<CruncherAppPreset> additionalApps = __instance?.controller?.ic?.interactable?.preset?.additionalApps;
        
        if (additionalApps == null)
        {
            return;
        }
        
        additionalApps.Clear();
        
        // Restore the original apps in reverse.
        for (int i = ReversedApps.Count - 1; i >= 0; --i)
        {
            additionalApps.Add(ReversedApps[i]);
        }
    }
}