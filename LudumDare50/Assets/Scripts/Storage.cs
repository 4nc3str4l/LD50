using UnityEngine;

public class Storage
{
    public static int GetNightsSurvived()
    {
        return PlayerPrefs.GetInt("NightsSurvived");
    }

    public static void SetNightsSurvived(int _x)
    {
        PlayerPrefs.SetInt("NightsSurvived", _x);
    }
}
