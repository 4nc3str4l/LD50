using UnityEngine;

public class AssasinSoulStorm : Skill
{

    private GameObject m_AssasinSoulPrefab;

    public AssasinSoulStorm(UISkill _ui, GameObject _prefab) : base("Death Storm", 100, 4, 5.0f, 10.0f, _ui)
    {
        m_AssasinSoulPrefab = _prefab;
    }

    protected override void OnActivate()
    {
        Guard[] guards = GameObject.FindObjectsOfType<Guard>();

        foreach(Guard g in guards)
        {
            if(g != null)
            {
                GameObject soul = GameObject.Instantiate(m_AssasinSoulPrefab);
                soul.GetComponent<AssasinSoul>().Init(Random.Range(1.0f, 3.0f), g);
            }
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