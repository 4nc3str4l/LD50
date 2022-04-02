using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Image Backgrund;
    public Image ForeGround;
    
    public Color EmptyColor = Color.red;
    public Color InitialColorFE;
    public Color InitialColorBG;

    public float Progress { get; private set; }

    private void Awake()
    {
        InitialColorFE = ForeGround.color;
        InitialColorBG = Backgrund.color;
    }

    public void SetProgress(float _progress)
    {
        ForeGround.fillAmount = _progress;
        Progress = _progress;

        if(_progress <= 0)
        {
            Backgrund.color = EmptyColor;
            ForeGround.color = EmptyColor;
        }
        else
        {
            Backgrund.color = InitialColorBG;
            ForeGround.color = InitialColorFE;
        }
    }

    public void Show()
    {
        Backgrund.gameObject.SetActive(true);
        ForeGround.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Backgrund.gameObject.SetActive(false);
        ForeGround.gameObject.SetActive(false);
    }

}
