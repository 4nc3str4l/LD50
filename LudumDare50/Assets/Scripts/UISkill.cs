using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UISkill : MonoBehaviour, IPointerDownHandler
{
    public Image SkillIcon;
    public TMP_Text TXtName;
    public TMP_Text TXtPrice;
    public TMP_Text TxtNumber;

    public Image ImgNotAffordable;
    public Image ActivateProgress;
    public Image CoolDownProgress;

    private Skill m_TrackingSkill;

    private Vector3 m_InitialScale;
    private Vector3 m_TxtPriceInitialScale;
    


    public void Init(Skill _skill, string _name, int _price, int _number)
    {
        m_TrackingSkill = _skill;
        TxtNumber.text = _number.ToString();
        TXtName.text = _name;
        TXtPrice.text = _price.ToString();
    }

    private void OnEnable()
    {
        m_InitialScale = transform.localScale;
        m_TxtPriceInitialScale = TXtPrice.transform.localScale;
        
        Skill.OnCantAffordSkill += Skill_OnCantAffordSkill;
        Skill.OnSkillActivated += Skill_OnSkillActivated;
        Skill.OnSkillActivatedInCoolDown += Skill_OnSkillActivatedInCoolDown;
        Skill.OnSkillDeactivated += Skill_OnSkillDeactivated;
    }

    private void OnDisable()
    {
        Skill.OnCantAffordSkill -= Skill_OnCantAffordSkill;
        Skill.OnSkillActivated -= Skill_OnSkillActivated;
        Skill.OnSkillActivatedInCoolDown -= Skill_OnSkillActivatedInCoolDown;
        Skill.OnSkillDeactivated -= Skill_OnSkillDeactivated;
    }

    private void Skill_OnCantAffordSkill(Skill _skill)
    {
        if(_skill != m_TrackingSkill)
        {
            return;
        }

        TXtPrice.transform.DOShakeScale(1).OnComplete(() =>
        {
            TXtPrice.transform.localScale = m_TxtPriceInitialScale;
        });
    }

    private void Skill_OnSkillDeactivated(Skill _skill)
    {
        if (_skill != m_TrackingSkill)
        {
            return;
        }
    }

    private void Skill_OnSkillActivatedInCoolDown(Skill _skill)
    {
        if (_skill != m_TrackingSkill)
        {
            return;
        }

        transform.DOShakeScale(1).OnComplete(() =>
        {
            transform.localScale = m_InitialScale;
        });
    }

    private void Skill_OnSkillActivated(Skill _skill)
    {
        if (_skill != m_TrackingSkill)
        {
            return;
        }
    }

    private void Update()
    {
        switch(m_TrackingSkill.Status)
        {
            case SkillStatus.ACTIVE:
                UpdateActiveSkill();
                break;
            case SkillStatus.COOOLING_DOWN:
                UpdateCoolingDownSkill();
                break;
            case SkillStatus.READY:
                UpdateReadySkill();
                break;
            default:
                break;
        }
    }

    private void UpdateActiveSkill()
    {
        DeactivateIfNotActive(CoolDownProgress);
        DeactivateIfNotActive(ImgNotAffordable);
        ActivateIfNotActive(ActivateProgress);
        ActivateProgress.fillAmount = 1 - m_TrackingSkill.ActiveProgress();
    }

    private void UpdateCoolingDownSkill()
    {
        DeactivateIfNotActive(ImgNotAffordable);
        DeactivateIfNotActive(ActivateProgress);
        ActivateIfNotActive(CoolDownProgress);
        CoolDownProgress.fillAmount = m_TrackingSkill.CoolDownProgress();
    }

    private void UpdateReadySkill()
    {
        DeactivateIfNotActive(CoolDownProgress);
        DeactivateIfNotActive(ActivateProgress);

        if(!m_TrackingSkill.CanAffond())
        {
            ActivateIfNotActive(ImgNotAffordable);
        }
        else
        {
            DeactivateIfNotActive(ImgNotAffordable);
        }
    }

    private void DeactivateIfNotActive(MonoBehaviour _cmp)
    {
        if (_cmp.gameObject.activeSelf)
        {
            _cmp.gameObject.SetActive(false);
        }
    }

    public void ActivateIfNotActive(MonoBehaviour _cmp)
    {
        if (!_cmp.gameObject.activeSelf)
        {
            _cmp.gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SkillManager.Instance.ActivateSkill(m_TrackingSkill);
    }
}
