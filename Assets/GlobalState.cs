using UnityEngine;
public class GlobalState
{
    public static bool isActivButtonBortAcum = false;
    public static bool isActivButtonBortSyst = false;
    public static bool isActivButtonHand = false;
    public static bool isActivButtonAvt = false;
    public static bool isActivButtonVK = false;
    public static bool isActivORK = true;

    public static bool IsPower()
    {
        Debug.Log(isActivButtonBortAcum && isActivButtonBortSyst);
        if (isActivButtonBortAcum && isActivButtonBortSyst)
            return true;

        return false;
    }

    public static bool IsActivBAvtBHand()
    {
        Debug.Log(isActivButtonBortAcum && isActivButtonBortSyst);
        if (isActivORK && isActivButtonAvt && isActivButtonHand && !isActivButtonVK)
            return true;

        return false;
    }

}