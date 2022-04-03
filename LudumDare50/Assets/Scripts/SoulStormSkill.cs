public class SoulStormSkill : Skill
{

    private float m_SpeedIncrease;

    public SoulStormSkill(UISkill _ui) : base("Soul Storm", 50, 1, 20.0f, 10.0f, _ui)
    { 

    }

    protected override void OnActivate()
    {
        m_SpeedIncrease = PlayerStats.Instance.Speed;
        PlayerStats.Instance.Speed += m_SpeedIncrease;
        Jukebox.Instance.PlaySound(Jukebox.Instance.Sprint, 0.6f);
    }


    protected override void OnUpdate()
    {
    }

    protected override void OnDeactivate()
    {
        PlayerStats.Instance.Speed -= m_SpeedIncrease;
    }

}