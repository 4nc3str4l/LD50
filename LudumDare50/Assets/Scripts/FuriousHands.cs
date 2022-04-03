using System;

public class FuriousHands : Skill
{

    private int ForceIncrease;

    public static event Action OnFuryStart;
    public static event Action OnFuryEnd;


    public FuriousHands(UISkill _ui) :
        base("Soul Fury", 3, 1, 5.0f, 10.0f, _ui)
    {
    }

    protected override void OnActivate()
    {
        ForceIncrease = PlayerStats.Instance.Strength * 5;
        PlayerStats.Instance.Strength += ForceIncrease;
        Jukebox.Instance.PlaySound(Jukebox.Instance.SoulFury, 0.7f);
        OnFuryStart?.Invoke();
    }


    protected override void OnUpdate()
    {
    }

    protected override void OnDeactivate()
    {
        PlayerStats.Instance.Strength -= ForceIncrease;
        OnFuryEnd?.Invoke();
    }

}