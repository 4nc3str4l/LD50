
using UnityEngine;
using DG.Tweening;

public class AlarmSystem : MonoBehaviour
{
    private AudioSource m_Source;
    private Material m_Material;
    private Light m_Light;

    private float m_EnabledUntil = 0;
    private float m_StartedIn  = 0;
    private float m_RingTime = 5.0f;
    
    public bool IsEnabled
    {
        get
        {
            return m_EnabledUntil >= Time.time;
        }
    }

    public float GetProgress()
    {
        float progress = (m_EnabledUntil - Time.time) / m_RingTime;
        return Mathf.Max(0, progress);
    }

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        m_Material = GetComponent<Renderer>().material;
        m_Light = GetComponentInChildren<Light>(true);
        m_Light.gameObject.SetActive(false);
    }

    private void Update()
    {
        m_Light.gameObject.SetActive(IsEnabled);
    }

    public void Ring()
    {
        m_StartedIn = Time.time;
        m_EnabledUntil = m_StartedIn + m_RingTime;
        Debug.Log("We sould play the alarm sound");
        Debug.Log("Make lights blink!");
    }

}
