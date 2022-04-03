public class Invisibility : Skill
{

    private float m_SpeedIncrease;

    public Invisibility(UISkill _ui) :
        base("Invisibility", 15, 1, 5.0f, 10.0f, _ui)
    }

    protected override void OnActivate()
    {
        PlayerStats.Instance.IsInvisible = true;
        Jukebox.Instance.PlaySound(Jukebox.Instance.Invisibility, 0.6f);
    }


    protected override void OnUpdate()
    {
    }

    protected override void OnDeactivate()
    {
        PlayerStats.Instance.IsInvisible = false;
    }

}