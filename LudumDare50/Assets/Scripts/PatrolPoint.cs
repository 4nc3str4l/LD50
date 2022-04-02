using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    private District m_District;
    public int IndexInArray;

    private void Awake()
    {
        m_District = GetComponentInParent<District>();
    }
}
