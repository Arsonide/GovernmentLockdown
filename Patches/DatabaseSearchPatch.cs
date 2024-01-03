using System;
using Il2CppSystem.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace GovernmentLockdown;

[HarmonyPatch(typeof(DatabaseApp), "UpdateSearch")]
public class DatabaseSearchPatch
{
    [HarmonyPrefix]
    private static bool Prefix(DatabaseApp __instance)
    {
        // This is going to have to be a carbon copy of the original, save for a few lines.
        List<ComputerOSMultiSelect.OSMultiOption> list = new List<ComputerOSMultiSelect.OSMultiOption>();
        string text = string.Empty;
        
        if (SearchIsValid(__instance.searchString))
        {
            List<Citizen> list2 = new List<Citizen>();
            string search = __instance.searchString.ToLower();
            NewGameLocation location = __instance?.controller?.ic?.interactable?.node?.gameLocation;
            
            switch (__instance.citizenPool)
            {
                case DatabaseApp.CitizenPool.allCitizens:
                    FindInAllCitizens(search, ref list2);
                    break;
                case DatabaseApp.CitizenPool.companyOnly:
                    FindInCompanyCitizens(search, location, ref list2);
                    break;
                case DatabaseApp.CitizenPool.buildingOnly:
                    FindInBuildingCitizens(search, location, ref list2);
                    break;
            }

            foreach (Citizen item in list2)
            {
                ComputerOSMultiSelect.OSMultiOption oSMultiOption = new ComputerOSMultiSelect.OSMultiOption();
                oSMultiOption.text = item.GetCitizenName();
                oSMultiOption.human = item;
                list.Add(oSMultiOption);
            }
        }
        else
        {
            text = "<color=\"red\">";
        }
        
        __instance.searchText.text = Strings.Get("computer", "Database Search") + ": " + text + __instance.searchString + "_";
        __instance.list.UpdateElements(list);
        __instance.UpdateSelected();

        // Override the original.
        return false;
    }

    private static void FindInAllCitizens(string search, ref List<Citizen> results)
    {
        foreach (Citizen citizen in CityData.Instance.citizenDirectory)
        {
            if (citizen.GetCitizenName().ToLower().Contains(search))
            {
                results.Add(citizen);
            }
        }
    }

    private static void FindInCompanyCitizens(string search, NewGameLocation location, ref List<Citizen> results)
    {
        foreach (Citizen citizen in CityData.Instance.citizenDirectory)
        {
            if (citizen?.job?.employer == null)
            {
                continue;
            }

            if (citizen.job.employer.address != location)
            {
                continue;
            }

            if (citizen.GetCitizenName().ToLower().Contains(search))
            {
                results.Add(citizen);
            }
        }
    }
    
    private static void FindInBuildingCitizens(string search, NewGameLocation location, ref List<Citizen> results)
    {
        foreach (Citizen citizen in CityData.Instance.citizenDirectory)
        {
            if (citizen?.home == null)
            {
                continue;
            }

            if (citizen.home.building != location.building)
            {
                continue;
            }

            if (citizen.GetCitizenName().ToLower().Contains(search))
            {
                results.Add(citizen);
            }
        }
    }

    private static bool SearchIsValid(string search)
    {
        // We are going to say a search is valid if it has 2 non-whitespace characters. This helps prevent the player from "brute forcing" the database.
        if (string.IsNullOrEmpty(search))
        {
            return false;
        }

        int nonWhiteSpace = 0;

        foreach (char c in search)
        {
            if (char.IsWhiteSpace(c))
            {
                continue;
            }

            nonWhiteSpace++;
            
            if (nonWhiteSpace >= 2)
            {
                return true;
            }
        }

        return false;
    }
}