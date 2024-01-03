using HarmonyLib;
using UnityEngine;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DatabaseApp), "OnSetup")]
public class MedDatabasePrintsBirthCertificatesPatch
{
    private static InteractablePreset _printedBirthCertificate = null;
    
    [HarmonyPostfix]
    private static void Postfix(DatabaseApp __instance)
    {
        // Set up the preset if it doesn't exist.
        if (_printedBirthCertificate == null)
        {
            _printedBirthCertificate = Object.Instantiate(Toolbox.Instance.GetInteractablePreset("BirthCertificate"));
            _printedBirthCertificate.readingEnabled = false;
            _printedBirthCertificate.attemptToStoreInFolder = null;

            _printedBirthCertificate.actionsPreset.Clear();
            
            // At this point, ddsPrintout will be the Citizen Report still from the government database.
            // Birth certificates are usually in files, and cannot be "taken", so we make them takeable by cloning the action list.
            foreach (InteractableActionsPreset action in __instance.ddsPrintout.actionsPreset)
            {
                _printedBirthCertificate.actionsPreset.Add(action);
            }
        }

        // Make the swap if we're in a hospital.
        switch (Utilities.GetMunicipalComputerType(__instance))
        {
            case MunicipalComputerType.HospitalInsecure:
            case MunicipalComputerType.HospitalSecure:
                __instance.ddsPrintout = _printedBirthCertificate;
                break;
        }
    }
}