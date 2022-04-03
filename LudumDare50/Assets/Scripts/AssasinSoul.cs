using DG.Tweening;
using UnityEngine;

public class AssasinSoul : MonoBehaviour
{
    public Guard Target;

    public void Init(float _travelTime, Guard _target)
    {
        transform.position = _target.transform.position + Vector3.up * 30.0f + new Vector3(Random.Range(-200f, 300f), 0, Random.Range(-300f, 240f));

        _target.Stun(10f);

        transform.DOMove(_target.transform.position, _travelTime).OnComplete(() =>
        {
            if (_target != null)
            {
                _target.SetDead();
                _target.transform.SetParent(transform);
            }

            transform.DOMove(_target.transform.position + Vector3.up * 30.0f + new Vector3(Random.Range(-200f, 300f), 0, Random.Range(-300f, 240f)), 2f).OnComplete(() => {
                GameObject.Destroy(gameObject);
            });
        
        });
    }

}
