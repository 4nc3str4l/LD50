using UnityEngine;

public class GuardSensor : MonoBehaviour
{
    private Guard m_Guard;

    private void Awake()
    {
        m_Guard = GetComponentInParent<Guard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (stats != null && !stats.IsInvisible)
        {
            m_Guard.SetChasing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            m_Guard.SetPatrol();
        }
    }
}
