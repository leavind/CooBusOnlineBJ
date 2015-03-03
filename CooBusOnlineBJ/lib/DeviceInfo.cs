using Microsoft.Phone.Info;
using System;

public class DeviceInfo
{
    public static string GetDeviceUniqueID()
    {
        byte[] result = null;
        object uniqueId;
        if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            result = (byte[])uniqueId;
        return BitConverter.ToString(result, 0, result.Length);
    }

    public static string GetWindowsLiveID()
    {
        string result = "";
        object anid;
        if (UserExtendedProperties.TryGetValue("ANID", out anid))
            if (anid != null && anid.ToString().Length >= 34)
                result = anid.ToString().Substring(2, 32);

        if (UserExtendedProperties.TryGetValue("ANID2", out anid))
            if (anid != null && anid.ToString().Length >= 34)
                result+= "\n"+anid.ToString().Substring(2, 32);
        return result.Trim();
    }

}