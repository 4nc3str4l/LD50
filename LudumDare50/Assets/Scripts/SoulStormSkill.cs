using UnityEngine;

public class SoulStormSkill : Skill
{


    public SoulStormSkill(UISkill _ui) : base("Soul Storm", 15, 4, 20.0f, 10.0f, _ui)
    {

    }

    protected override void OnActivate()
    {
        Guard[] guards = GameObject.FindObjectsOfType<Guard>();

        foreach(Guard g in guards)
        {
            g.Stun(20);
        }

        WeatherManager.Instance.SetSunlightColor(Color.red, 1.2f);
        Jukebox.Instance.PlaySound(Jukebox.Instance.SoulStorm, 0.6f);
    }


    protected override void OnUpdate()
    {
    }

    protected override void OnDeactivate()
    {
        WeatherManager.Instance.SetSunToNormal(0.4f);
    }

}