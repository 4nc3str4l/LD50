using UnityEngine;
using DG.Tweening;

public class UIDayTime : MonoBehaviour
{

    private CanvasGroup m_CanvasGroup;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        GameController.OnDayStarted += GameController_OnDayStarted;
        GameController.OnNightStarted += GameController_OnNightStarted;
    }

    private void GameController_OnNightStarted()
    {
        m_CanvasGroup.DOFade(0, 1);
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
    }

    private void GameController_OnDayStarted()
    {
        m_CanvasGroup.DOFade(1, 1);
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
    }

    private void OnDisable()
    {
        GameController.OnDayStarted -= GameController_OnDayStarted;
        GameController.OnNightStarted -= GameController_OnNightStarted;
    }
}
