using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public GameObject AssasinSoulPrefab;

    public UISkill UIFurySkill;
    private FuriousHands m_FurySkill;

    public UISkill UISprint;
    private SprintSkill m_Sprint;

    public UISkill UIInvisibilitySkill;
    private InvisibilitySkill m_InvisibilitySkill;

    public UISkill UISoulStorm;
    private SoulStormSkill m_SoulStorm;

    public UISkill UIAssasinSoulStorm;
    private AssasinSoulStorm m_AssasinSoulStorm;

    private List<Skill> m_SkillsToDeactivate = new List<Skill>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_FurySkill = new FuriousHands(UIFurySkill);
        m_Sprint = new SprintSkill(UISprint);
        m_InvisibilitySkill = new InvisibilitySkill(UIInvisibilitySkill);
        m_SoulStorm = new SoulStormSkill(UISoulStorm);
        m_AssasinSoulStorm = new AssasinSoulStorm(UIAssasinSoulStorm, AssasinSoulPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateFury();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSprint();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateInvisibility();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateSoulStorm();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ActivateAssasinSoulStorm();
        }

        UpdateActiveSkills();
    }

    public void ActivateFury()
    {
        ActivateSkill(m_FurySkill);
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

    public void ActivateAssasinSoulStorm()
    {
        ActivateSkill(m_AssasinSoulStorm);
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
            if (skill.IsActive() && GameController.Instance.GameState !=  GameState.DAY)
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
