using UnityEngine;

public class BuildingHitPoint : MonoBehaviour
{
    public Bulding Owner;

    private void Start()
    {
        Owner = GetComponentInParent<Bulding>();
    }
}
