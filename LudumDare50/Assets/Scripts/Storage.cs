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

    public static bool GetTutorialEnabled()
    {
        if (!PlayerPrefs.HasKey("TutorialEnabled"))
        {
            return true;
        }

        return PlayerPrefs.GetInt("TutorialEnabled") == 1;
    }

    public static void SetTutorialEnabled(bool _enabled)
    {
        PlayerPrefs.SetInt("TutorialEnabled", _enabled ? 1 : 0);
    }



}
