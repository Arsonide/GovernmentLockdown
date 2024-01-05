using BepInEx.Logging;

namespace GovernmentLockdown;

public static class Utilities
{
    public const bool DEBUG_BUILD = false;
    
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
    
    public static MunicipalComputerType GetMunicipalComputerType(CruncherAppContent app)
    {
        NewNode node = app?.controller?.ic?.interactable?.node;
        AddressPreset addressPreset = node?.gameLocation?.thisAsAddress?.addressPreset;
        RoomConfiguration roomPreset = node?.room?.preset;
        
        if (node == null || addressPreset == null || roomPreset == null)
        {
            return MunicipalComputerType.NotMunicipal;
        }

        bool isSecure = roomPreset.escalationLevelNormal != 0;

        switch (addressPreset.presetName)
        {
            case "CityHallLobby":
                return isSecure ? MunicipalComputerType.LobbySecure : MunicipalComputerType.LobbyInsecure;
            case "EnforcerOffice":
                return isSecure ? MunicipalComputerType.EnforcersSecure : MunicipalComputerType.EnforcersInsecure;
            case "HospitalWard":
                return isSecure ? MunicipalComputerType.HospitalSecure : MunicipalComputerType.HospitalInsecure;
        }

        return MunicipalComputerType.NotMunicipal;
    }
}