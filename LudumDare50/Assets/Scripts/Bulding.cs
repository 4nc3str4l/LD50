using UnityEngine;
using DG.Tweening;
using System;

public class Bulding : MonoBehaviour
{
    public int DefensePoints = 3;
    public int StartDefensePoints = 3;
    public int NumHumans = 1;
    public bool HumansInside = true;
    public bool Alarm = false;
    public PatrolPoint ClosestPoint;

    private District m_OwnerDistrict;

    public GameObject AlarmGo;
    public GameObject UIPos;

    private Vector3 m_OriginalPos;

    public AlarmSystem AlarmSys;

    public static event Action<Bulding, int> OnBuldingKilled;

    private void Start()
    {
        AlarmSys = GetComponentInChildren<AlarmSystem>(true);
        m_OwnerDistrict = GetComponentInParent<District>();
        InGameUI.Instance.SpawnHomeUI(this);
        m_OriginalPos = transform.position;
    }

    private void OnEnable()
    {
        GameController.OnNightStarted += GameController_OnNightStarted;
    }

    private void OnDisable()
    {
        GameController.OnNightStarted -= GameController_OnNightStarted;
    }

    private void GameController_OnNightStarted()
    {

        float rHumans = UnityEngine.Random.Range(0, 1.0f);

        if(rHumans < 0.5f)
        {
            NumHumans = 1;
        }
        else if(rHumans >= 0.5f && rHumans < 0.8f)
        {
            NumHumans = 2;
        }
        else if(rHumans >= 0.8f && rHumans < 0.95f)
        {
            NumHumans = 3;
        }
        else if (rHumans >= 0.95f && rHumans < 0.99f)
        {
            NumHumans = 6;
        }
        else
        {
            NumHumans = 5;
        }

        AlarmGo.SetActive(Alarm);

        // Skip buldings that haven't been attacked
        if (HumansInside)
        {
            return;
        }

        StartDefensePoints += UnityEngine.Random.Range(2, 10);
        DefensePoints = StartDefensePoints;
        HumansInside = true;

        if(!Alarm)
        {
            if (UnityEngine.Random.Range(0, 1.0f) > 0.8f)
            {
                Alarm = true;
            }
        }

        AlarmGo.SetActive(Alarm);

        // Inform the district about the attack
        m_OwnerDistrict.OnHomeAttacked();
    }


    public void GetAttacked(int _dmg)
    {
        if (DoorDestroyed())
        {
            return;
        }

        DefensePoints -= _dmg;

        if(Alarm)
        {
            m_OwnerDistrict.BuldingUnderAttack(ClosestPoint.IndexInArray);
            AlarmSys.Ring();
        }

        if(DefensePoints <= 0)
        {
            DefensePoints = 0;
            HumansInside = false;

            Scheduler.Instance.ExecuteIn(() =>
            {
                GameController.Instance.FireOnHumanKilled(NumHumans);
                OnBuldingKilled?.Invoke(this, NumHumans);
            }, 1.5f);

            Jukebox.Instance.PlaySound(Jukebox.Instance.DoorBreak, 0.6f);
            Jukebox.Instance.PlayRandomPeapoleReaction(0.5f, 0.3f);
            Jukebox.Instance.PlaySoundDelayed(Jukebox.Instance.Crunch, 0.5f, 1.25f);
            Jukebox.Instance.PlayRandomAttack(0.5f, 0.9f);
        }
        else
        {
            Jukebox.Instance.PlayRandomDoorHit(0.7f);
        }

        transform.DOShakePosition(0.1f, 0.1f).OnComplete(() =>
        {
            transform.position = m_OriginalPos;
        });
    }

    public bool DoorDestroyed()
    {
        return DefensePoints == 0;
    }

}
