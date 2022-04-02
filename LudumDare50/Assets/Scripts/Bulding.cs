using UnityEngine;
using DG.Tweening;

public class Bulding : MonoBehaviour
{
    public int DefensePoints = 10;
    public bool HumansInside = true;
    public bool Alarm = false;

    public GameObject UIPos;

    private Vector3 m_OriginalPos;

    private void Start()
    {
        InGameUI.Instance.SpawnHomeUI(this);
        m_OriginalPos = transform.position;
    }

    public void GetAttacked(int _dmg)
    {
        DefensePoints -= _dmg;

        if(DefensePoints < 0)
        {
            DefensePoints = 0;
        }


        transform.DOShakePosition(0.1f, 0.1f).OnComplete(() =>
        {
            transform.position = m_OriginalPos;
        });
    }
}
