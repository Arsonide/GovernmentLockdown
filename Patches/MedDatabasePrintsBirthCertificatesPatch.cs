using HarmonyLib;
using TMPro;
using UnityEngine;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DatabaseApp), "OnSetup")]
public class MedDatabasePrintsBirthCertificatesPatch
{
    private static InteractablePreset _printedCitizensReport;
    private static InteractablePreset _printedEmployeeReport;
    private static InteractablePreset _printedResidentReport;
    private static InteractablePreset _printedBirthCertificate;

    [HarmonyPostfix]
    private static void Postfix(DatabaseApp __instance)
    {
        CachePresets();

        ComputerAutoTextController autoText = __instance.titleText.gameObject.GetComponent<ComputerAutoTextController>();

        if (autoText != null)
        {
            Object.DestroyImmediate(autoText);
        }

        __instance.titleText.overflowMode = TextOverflowModes.Overflow;
        __instance.titleText.enableWordWrapping = false;

        switch (__instance.citizenPool)
        {
            case DatabaseApp.CitizenPool.allCitizens:
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

                break;
            case DatabaseApp.CitizenPool.companyOnly:
                __instance.titleText.text = "Employee Database";
                __instance.ddsPrintout = _printedEmployeeReport;
                break;
            case DatabaseApp.CitizenPool.buildingOnly:
                __instance.titleText.text = "Residents Database";
                __instance.ddsPrintout = _printedResidentReport;
                break;
        }
    }

    private static void CachePresets()
    {
        // Set up the presets if they don't exist. We're just going to handle all of them since there seems to be some pooling occurring.
        if (_printedCitizensReport == null)
        {
            _printedCitizensReport = Toolbox.Instance.GetInteractablePreset("PrintedCitizenFile");
        }
        
        if (_printedEmployeeReport == null)
        {
            _printedEmployeeReport = Toolbox.Instance.GetInteractablePreset("PrintedEmployeeRecord");
        }
        
        if (_printedResidentReport == null)
        {
            _printedResidentReport = Toolbox.Instance.GetInteractablePreset("PrintedResidentsFile");
        }

        if (_printedBirthCertificate == null && _printedCitizensReport != null)
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
    }
}