using UnityEngine;
using DG.Tweening;

public class Bulding : MonoBehaviour
{
    public int DefensePoints = 3;
    public int StartDefensePoints = 3;
    public bool HumansInside = true;
    public bool Alarm = false;

    private District m_OwnerDistrict;

    public GameObject UIPos;

    private Vector3 m_OriginalPos;

    private void Start()
    {
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
        // Skip buldings that haven't been attacked
        if(HumansInside)
        {
            return;
        }

        StartDefensePoints += Random.Range(2, 10);
        DefensePoints = StartDefensePoints;
        HumansInside = true;

        if(Random.Range(0, 1.0f) > 0.8f)
        {
            Alarm = true;
        }

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

        if(DefensePoints <= 0)
        {
            DefensePoints = 0;
            HumansInside = false;
            GameController.Instance.FireOnHumanKilled();
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
