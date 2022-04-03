using TMPro;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    public TMP_Text TxtNightsSurvived;

    public AudioClip GameOver;
    public AudioSource AudioSource;

    private void Start()
    {
        TxtNightsSurvived.text = Storage.GetNightsSurvived().ToString();
        AudioSource.PlayOneShot(GameOver, VolumeMaster.Instance.Volume * 0.7f);
    }
}
