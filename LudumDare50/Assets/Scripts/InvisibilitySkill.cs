using System;
using UnityEngine;

public class InvisibilitySkill : Skill
{

    public static event Action OnInvisibilityStarted;
    public static event Action OnInvisibilityEnded;



    public InvisibilitySkill(UISkill _ui) : base("Invisibility", 7, 3, 5.0f, 10.0f, _ui)
    {

    }

    protected override void OnActivate()
    {
        PlayerStats.Instance.IsInvisible = true;
        Jukebox.Instance.PlaySound(Jukebox.Instance.Invisibility, 0.6f);
        OnInvisibilityStarted?.Invoke();
    }


    protected override void OnUpdate()
    {
    }

    protected override void OnDeactivate()
    {
        PlayerStats.Instance.IsInvisible = false;
        OnInvisibilityEnded?.Invoke();
    }

}