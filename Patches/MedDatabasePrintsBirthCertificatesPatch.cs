using HarmonyLib;
using TMPro;
using UnityEngine;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DatabaseApp), "OnSetup")]
public class MedDatabasePrintsBirthCertificatesPatch
{
    private static InteractablePreset _printedCitizensReport = null;
    private static InteractablePreset _printedBirthCertificate = null;

    [HarmonyPostfix]
    private static void Postfix(DatabaseApp __instance)
    {
        if (Utilities.GetMunicipalComputerType(__instance) == MunicipalComputerType.NotMunicipal)
        {
            return;
        }
        
        // Set up the presets if they don't exist.
        if (_printedCitizensReport == null)
        {
            _printedCitizensReport = __instance.ddsPrintout;
        }
        
        if (_printedBirthCertificate == null)
        {
            _printedBirthCertificate = Object.Instantiate(Toolbox.Instance.GetInteractablePreset("BirthCertificate"));
            _printedBirthCertificate.readingEnabled = false;
            _printedBirthCertificate.attemptToStoreInFolder = null;

            _printedBirthCertificate.actionsPreset.Clear();
            
            // At this point, ddsPrintout will be the Citizen Report still from the government database.
            // Birth certificates are usually in files, and cannot be "taken", so we make them takeable by cloning the action list.
            foreach (InteractableActionsPreset action in _printedCitizensReport.actionsPreset)
            {
                _printedBirthCertificate.actionsPreset.Add(action);
            }
        }

        ComputerAutoTextController autoText = __instance.titleText.gameObject.GetComponent<ComputerAutoTextController>();

        if (autoText != null)
        {
            Object.DestroyImmediate(autoText);
        }
        
        __instance.titleText.overflowMode = TextOverflowModes.Overflow;
        __instance.titleText.enableWordWrapping = false;
        
        // Make the swap if we're in a hospital.
        switch (Utilities.GetMunicipalComputerType(__instance))
        {
            case MunicipalComputerType.HospitalInsecure:
            case MunicipalComputerType.HospitalSecure:
                __instance.titleText.text = "Medical Database";
                __instance.ddsPrintout = _printedBirthCertificate;
                break;
            default:
                __instance.titleText.text = "Citizens Database";
                __instance.ddsPrintout = _printedCitizensReport;
                break;
        }
    }
}