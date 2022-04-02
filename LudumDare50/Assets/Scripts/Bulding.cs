using UnityEngine;

public class Bulding : MonoBehaviour
{
    public int DefensePoints = 10;
    public bool HumansInside = true;
    public bool Alarm = false;

    public GameObject UIPos;

    private void Start()
    {
        InGameUI.Instance.SpawnHomeUI(this);
    }

}
