using UnityEngine;
using DG.Tweening;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Door;

    public static Portal Instance;

    public GameObject SoulPrefab;
    public Vector3 m_InitialPosition;

    private void Awake()
    {
        Instance = this;
        m_InitialPosition = transform.position;
    }

    private void OnEnable()
    {
        Bulding.OnBuldingKilled += Bulding_OnBuldingKilled;
    }

    private void OnDisable()
    {
        Bulding.OnBuldingKilled -= Bulding_OnBuldingKilled;
    }

    private void Bulding_OnBuldingKilled(Bulding _target, int _numKilled)
    {
        for(int i = 0; i < _numKilled; ++i)
        {
            GameObject sp = GameObject.Instantiate(SoulPrefab);
            sp.transform.position = _target.transform.position + Vector3.up * 4.0f;
            sp.GetComponent<Soul>().Init(Random.Range(5.0f, 10.0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerStats>() != null)
        {
            if (CanEnterPortal())
            {
                Debug.Log("Player In Portal");
                GameController.Instance.StartDay();
                Jukebox.Instance.PlaySound(Jukebox.Instance.PortalEnter, 0.7f);
            }
            else
            {
                Debug.Log("You can't enter yet, kill some humans first");
            }
        }
    }


    private void Update()
    {
        if(Vector3.Distance(transform.position, PlayerStats.Instance.transform.position) < 42 && CanEnterPortal())
        {
            if (!Door.activeSelf)
            {
                Debug.Log("Enabling Portal");
                Door.SetActive(true);
            }
        }
        else
        {
            if (Door.activeSelf)
            {
                Debug.Log("Disabling Portal");
                Door.SetActive(false);
            }
        }
    }

    public bool CanEnterPortal()
    {
        return GameController.Instance.MissingSoulsToCollect <= 0;
    }


    public void Shake()
    {
        transform.DOShakePosition(1.0f).OnComplete(() => {
            transform.position = m_InitialPosition;
        });
    }

}
