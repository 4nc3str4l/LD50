using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public float Health = 100.0f;
    public int SoulsToCollect = 0;
    public float Speed = 200.0f;
    public float NightDuration = 120f;
    public int Strength = 1;
    public float Resistence = 0.0f;

    private void Awake()
    {
        Instance = this;
    }
}
