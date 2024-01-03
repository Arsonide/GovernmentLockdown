using System;
using HarmonyLib;
using UnityEngine;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DatabaseApp), "OnPrintEntry")]
public class MedDatabasePrintingPatch
{
    [HarmonyPrefix]
    private static bool Prefix(DatabaseApp __instance)
    {
        MunicipalComputerType municipalType = Utilities.GetMunicipalComputerType(__instance);

        switch (municipalType)
        {
            case MunicipalComputerType.HospitalSecure:
            case MunicipalComputerType.HospitalInsecure:
                // Do nothing, we need to override the printing behavior.
                break;
            default:
                return true;
        }

        // Normal printing always shows the player as the "owner" which on Birth Certificates makes them the subject.
        // We need to override this behavior now, so we return false and have our own print method.
        OnMedicalDatabasePrint(__instance);
        return false;
    }

    private static void OnMedicalDatabasePrint(DatabaseApp app)
    {
        // This entire method is mostly a cleaned up version of the original, save one line.
        if (app.selectedHuman == null)
        {
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerInvalidPasscode, Player.Instance, app.controller.ic.interactable.node, app.controller.ic.interactable.wPos);
            return;
        }

        Game.Log("Print " + app.selectedHuman?.ToString());
        app.controller.SetTimedLoading(Toolbox.Instance.Rand(0.5f, 1f));

        if (app.controller.printedDocument != null || app.controller.printTimer > 0f)
        {
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerInvalidPasscode, Player.Instance, app.controller.ic.interactable.node, app.controller.ic.interactable.wPos);
            return;
        }

        app.controller.printTimer = 1f;
        Vector3 printerParentPosition = app.controller.printerParent.localPosition;
        app.controller.printerParent.localPosition = new Vector3(printerParentPosition.x, printerParentPosition.y, -0.05f);
        AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.computerPrint, Player.Instance, app.controller.ic.interactable.node, app.controller.ic.interactable.wPos);

        // This is the line that's different, we need belongsTo to be the NPC as well, not the player.
        app.controller.printedDocument = InteractableCreator.Instance.CreateWorldInteractable(app.ddsPrintout, app.selectedHuman, app.selectedHuman, app.selectedHuman, app.controller.printerParent.position,
                                                                                              app.controller.ic.transform.eulerAngles, null, null);

        if (app.controller.printedDocument != null)
        {
            app.controller.printedDocument.MarkAsTrash(val: true);
        }

        Action OnTakePrint = app.OnPlayerTakePrint;
        app.controller.printedDocument.OnRemovedFromWorld += OnTakePrint;
    }
}