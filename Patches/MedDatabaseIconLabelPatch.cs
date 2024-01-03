using HarmonyLib;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DesktopIconController), "Setup")]
public class MedDatabaseIconLabelPatch
{
    [HarmonyPostfix]
    private static void Postfix(DesktopIconController __instance, DesktopApp newDesktop, CruncherAppPreset newApp)
    {
        if (newApp.presetName != "GovDatabase")
        {
            return;
        }
        
        switch (Utilities.GetMunicipalComputerType(newDesktop))
        {
            case MunicipalComputerType.HospitalSecure:
            case MunicipalComputerType.HospitalInsecure:
                __instance.iconText.text = "Med Database";
                break;
        }
    }
}