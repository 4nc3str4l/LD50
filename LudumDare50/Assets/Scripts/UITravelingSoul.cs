using UnityEngine;
using DG.Tweening;

public class UITravelingSoul : MonoBehaviour
{

    private Transform m_Target;
    private float m_TimeToTravel;
    
    public void Init(Bulding _origin, float _timeToTravel, Transform _target)
    {
        Vector3 originalPosition = Camera.main.WorldToScreenPoint(_origin.UIPos.transform.position);
        Vector3 randomizedPosition = originalPosition + new Vector3(Random.Range(-40, 40), Random.Range(-40, 40), 0);
        transform.position = originalPosition;
        m_Target = _target;
        m_TimeToTravel = _timeToTravel;

        transform.DOMove(randomizedPosition, _timeToTravel / 3).OnComplete(() => {
            transform.DOMove(_target.transform.position, _timeToTravel / 3 * 2).OnComplete(() => {
                Destroy(gameObject, 0.05f);
            });
        });
    }
}
