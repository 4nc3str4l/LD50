using TMPro;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    public TMP_Text TxtNightsSurvived;

    private void Start()
    {
        TxtNightsSurvived.text = Storage.GetNightsSurvived().ToString();
    }
}
