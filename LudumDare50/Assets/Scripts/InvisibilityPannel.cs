using DG.Tweening;
using UnityEngine;

public class InvisibilityPannel : MonoBehaviour
{

    private CanvasGroup m_Group;

    private void Awake()
    {
        m_Group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        InvisibilitySkill.OnInvisibilityStarted += InvisibilitySkill_OnInvisibilityStarted;
        InvisibilitySkill.OnInvisibilityEnded += InvisibilitySkill_OnInvisibilityEnded;
    }

    private void OnDisable()
    {
        InvisibilitySkill.OnInvisibilityStarted -= InvisibilitySkill_OnInvisibilityStarted;
        InvisibilitySkill.OnInvisibilityEnded -= InvisibilitySkill_OnInvisibilityEnded;
    }
    private void InvisibilitySkill_OnInvisibilityStarted()
    {
        Show();
    }

    private void InvisibilitySkill_OnInvisibilityEnded()
    {
        Hide();
    }

    public void Show()
    {
        m_Group.DOFade(1, 0.3f);
    }


    public void Hide()
    {
        m_Group.DOFade(0, 0.5f);
    }
}
