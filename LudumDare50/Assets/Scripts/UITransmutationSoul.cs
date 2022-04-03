using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UITransmutationSoul : MonoBehaviour
{

    private Transform m_Target;
    private float m_TimeToTravel;

    private bool m_IsInit = false;

    private Image m_Sprite;

    private Color m_Initial;
    private Color m_Final;
    private float m_StartMovementTime;


    private void Awake()
    {
        m_Sprite = GetComponent<Image>();
        m_Initial = m_Sprite.color;
    }

    private void Update()
    {
        if (!m_IsInit)
        {
            return;
        }

        float ratio = (Time.time - m_StartMovementTime) / m_TimeToTravel;
        ratio = Mathf.Min(1, ratio);
        m_Sprite.color = Color.Lerp(m_Initial, m_Final, ratio);
    }

    public void Init(Transform _origin, float _timeToTravel, Transform _target, Action _onComplete)
    {
        Vector3 originalPosition = _origin.position;
        Vector3 randomizedPosition = originalPosition + new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), 0);
        transform.position = originalPosition;
        m_Target = _target;
        m_TimeToTravel = _timeToTravel;

        m_Final = m_Target.GetComponent<Image>().color;
        m_StartMovementTime = Time.time;

        m_IsInit = true;

        transform.DOMove(randomizedPosition, _timeToTravel / 3).OnComplete(() => {
            transform.DOMove(_target.transform.position, _timeToTravel / 3 * 2).OnComplete(() => {
                Destroy(gameObject, 0.05f);
                _onComplete?.Invoke();
            });
        });
    }
}
