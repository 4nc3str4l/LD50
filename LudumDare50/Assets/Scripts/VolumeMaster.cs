using System.Collections;
using UnityEngine;

public class VolumeMaster : MonoBehaviour
{

    public static VolumeMaster Instance;

    public float Volume { get; private set; }

    private void Awake()
    {
        Instance = this;

        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }

        Volume = PlayerPrefs.GetFloat("Volume");
    }

    public void SetVolume(float _volumne)
    {
        Debug.Log("Volume changed to: " + _volumne);
        Volume = _volumne;
        PlayerPrefs.SetFloat("Volume", _volumne);
    }
}
