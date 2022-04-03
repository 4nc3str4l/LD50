using System.Collections;
using UnityEngine;


public class StuckDetector : MonoBehaviour
{

    private Vector3 m_PositionSaved = Vector3.zero;

    private float m_CheckRate = 5.0f;
    private float m_NextCheck = 0;

    private Collider m_Collider;

    // Use this for initialization
    void Start()
    {
        m_Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < m_NextCheck)
        {
            return;
        }

        if (transform.position == m_PositionSaved)
        {
            m_Collider.isTrigger = true;
            Scheduler.Instance.ExecuteIn(() =>
            {
                if(m_Collider != null)
                {
                    m_Collider.isTrigger = false;
                }
            }, 10f);

        }
        else
        {
        }
        m_PositionSaved = transform.position;
        m_NextCheck = Time.time + m_CheckRate;
    }
}
