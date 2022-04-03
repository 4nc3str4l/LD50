using UnityEngine;

public abstract class Skill
{

    public int Price
    {
        get
        {
            return m_Price;
        }
    }

    private string m_Name;
    private int m_Price;
    private int m_Number;
    private float m_Duration;

    private UISkill m_UISkill;

    private float m_CoolDown = 0;
    private float m_FireTime = 0;
    private float m_CoolingDownUntil = 0;

    public SkillStatus Status
    {
        get
        {
            if (IsActive())
            {
                return SkillStatus.ACTIVE;
            }
            else if(IsInCooldown())
            {
                return SkillStatus.COOOLING_DOWN;
            }
            else
            {
                return SkillStatus.READY;
            }
        }
    }

    public delegate void SkillHandler(Skill _skill);
    public static event SkillHandler OnSkillActivated;
    public static void FireOnSkillActivated(Skill _skill)
    {
        OnSkillActivated?.Invoke(_skill);
    }

    public static event SkillHandler OnSkillActivatedInCoolDown;
    public static void FireOnSkillActivatedInCoolDown(Skill _skill)
    {
        OnSkillActivatedInCoolDown?.Invoke(_skill);
    }
    public static event SkillHandler OnCantAffordSkill;
    public static void FireOnCantAffordSkill(Skill _skill)
    {
        OnCantAffordSkill?.Invoke(_skill);
    }
    public static event SkillHandler OnSkillDeactivated;
    public static void FireOnSkillDeactivated(Skill _skill)
    {
        OnSkillDeactivated?.Invoke(_skill);
    }

    public Skill(string _name, int _price, int _number,  float _duration, float _coolDown, UISkill _ui)
    {
        m_FireTime = -_duration;
        m_CoolDown = _coolDown;
        m_Name = _name;
        m_Price = _price;
        m_Number = _number;
        m_Duration = _duration;
        m_UISkill = _ui;
        _ui.Init(this, _name, _price, _number);
    }

    public void Activate()
    {
        Debug.Log("Skill " + m_Name + " activated!");
        m_FireTime = Time.time;
        m_CoolingDownUntil = Time.time + m_Duration + m_CoolDown;
        OnActivate();
    }

    public bool IsActive()
    {
        return m_FireTime + m_Duration >= Time.time;
    }

    public float ActiveProgress()
    {
        return (Time.time - m_FireTime) / m_Duration;
    }

    public float CoolDownProgress()
    {
        return (m_CoolingDownUntil - Time.time) / m_CoolDown;
    }

    public void Deactivate()
    {
        Debug.Log("Skill " + m_Name + " deactivated!");
        OnDeactivate();
    }

    protected abstract void OnUpdate();
    protected abstract void OnActivate();
    protected abstract void OnDeactivate();

    public void Update()
    {
        OnUpdate();
    }


    public bool CanAffond()
    {
        return GameController.Instance.Bank.HasEnough(m_Price);
    }

    public bool IsInCooldown()
    {
        return Time.time <= m_CoolingDownUntil;
    }

    public bool CanActivate()
    {
        return CanAffond() && !IsInCooldown();
    }


}
