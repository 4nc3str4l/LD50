public class SprintSkill : Skill
{

    private float m_SpeedIncrease;

    public SprintSkill(UISkill _ui) :
        base("Sprint", 3, 1, 5.0f, 10.0f, _ui)
    {
    }

    protected override void OnActivate()
    {
        m_SpeedIncrease = PlayerStats.Instance.Speed;
        PlayerStats.Instance.Speed += m_SpeedIncrease;
    }


    protected override void OnUpdate()
    {
    }

    protected override void OnDeactivate()
    {
        PlayerStats.Instance.Speed -= m_SpeedIncrease;
    }

}