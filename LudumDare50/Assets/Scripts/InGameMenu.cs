using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{

    public Slider VolumeSlider;
    private CanvasGroup m_CanvasGroup;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        VolumeSlider.value = VolumeMaster.Instance.Volume;
    }

    public void OnVolumeChanged(float _volumne)
    {
        VolumeMaster.Instance.SetVolume(_volumne);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_CanvasGroup.interactable)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void Show()
    {
        m_CanvasGroup.DOFade(1, 1);
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        m_CanvasGroup.DOFade(0, 1);
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
    }
}
