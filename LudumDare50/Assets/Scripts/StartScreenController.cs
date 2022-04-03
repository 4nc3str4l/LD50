using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    public Button UIButton;

    public void OnStartClicked()
    {
        UIButton.interactable = false;
        UIButton.transform.DOShakeScale(1.0f);
    }
}
