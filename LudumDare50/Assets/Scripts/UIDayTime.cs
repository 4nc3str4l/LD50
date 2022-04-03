using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIDayTime : MonoBehaviour
{

    private CanvasGroup m_CanvasGroup;
    public TMP_Text TxtNightsSurvied;
    public TMP_Text TxtSecurityUpdate;

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
        TxtNightsSurvied.text = Storage.GetNightsSurvived().ToString();
        TxtSecurityUpdate.text = GenerateReport();
    }

    private string GenerateReport()
    {
        string report = "";
        foreach(District d in District.AttackedDistrics)
        {
            report += d.GetSecurityReport() + "\n";
        }
        return report;
    }

    private void OnDisable()
    {
        GameController.OnDayStarted -= GameController_OnDayStarted;
        GameController.OnNightStarted -= GameController_OnNightStarted;
    }
}
