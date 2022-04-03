using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public UISkill UISprint;
    private SprintSkill m_Sprint;

    public UISkill UIInvisibilitySkill;
    private InvisibilitySkill m_InvisibilitySkill;

    public UISkill UISoulStorm;
    private SoulStormSkill m_SoulStorm;

    private List<Skill> m_SkillsToDeactivate = new List<Skill>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_Sprint = new SprintSkill(UISprint);
        m_InvisibilitySkill = new InvisibilitySkill(UIInvisibilitySkill);
        m_SoulStorm = new SoulStormSkill(UISoulStorm);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateSprint();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateInvisibility();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateSoulStorm();
        }

        UpdateActiveSkills();
    }

    public void ActivateSprint()
    {
        ActivateSkill(m_Sprint);
    }


    public void ActivateInvisibility()
    {
        ActivateSkill(m_InvisibilitySkill);
    }


    public void ActivateSoulStorm()
    {
        ActivateSkill(m_SoulStorm);
    }

    public void ActivateSkill(Skill _skill)
    {
        if(_skill.CanActivate())
        {
            _skill.Activate();
            m_SkillsToDeactivate.Add(_skill);
            GameController.Instance.Bank.WithDraw(_skill.Price);
        }
        else if(!_skill.CanAffond())
        {
            Skill.FireOnCantAffordSkill(_skill);
        }
        else if(_skill.IsInCooldown())
        {
            Skill.FireOnSkillActivatedInCoolDown(_skill);
        }
    }

    private void UpdateActiveSkills()
    {
        Skill skill;
        for(int i = m_SkillsToDeactivate.Count -1; i >= 0; --i)
        {
            skill = m_SkillsToDeactivate[i];
            if (skill.IsActive())
            {
                skill.Update();
            }
            else
            {
                Skill.FireOnSkillDeactivated(skill);
                skill.Deactivate();
                m_SkillsToDeactivate.RemoveAt(i);
            }
        }
    }
}
