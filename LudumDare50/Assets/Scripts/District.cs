using UnityEngine;

public class District : MonoBehaviour
{
    public int NumWatchersToSpawn = 0;
    public int KillCounter = 0;
    public int GuardKillCounter = 0;

    public PatrolPoint[] PatrolPoints;

    public GameObject GuardPrefab;

    private void Awake()
    {
        for(int i = 0; i < PatrolPoints.Length; ++i)
        {
            PatrolPoints[i].IndexInArray = i;
        }
    }

    private void Start()
    {
        KillCounter = 0;
        GuardKillCounter = 0;
    }


    private void OnEnable()
    {
        GameController.OnDayStarted += GameController_OnDayStarted;
        GameController.OnNightStarted += GameController_OnNightStarted;
    }



    private void OnDisable()
    {
        GameController.OnDayStarted -= GameController_OnDayStarted;
        GameController.OnNightStarted -= GameController_OnNightStarted;
    }

    private void GameController_OnNightStarted()
    {
        for(int i = 0; i < NumWatchersToSpawn; ++i)
        {
            SpawnGuard();
        }
    }

    private void GameController_OnDayStarted()
    {

    }

    public void OnHomeAttacked()
    {
        KillCounter += 1;
        NumWatchersToSpawn += 1;
    }

    public void SpawnGuard()
    {
        GameObject g = GameObject.Instantiate(GuardPrefab);
        PatrolPoint p = PatrolPoints[Random.Range(0, PatrolPoints.Length)];
        g.transform.position = p.transform.position;
        g.GetComponent<Guard>().Init(this, p);
    }

    public void OnGuardAttacked()
    {
        GuardKillCounter += 1;
    }

    public PatrolPoint ChooseNextPatrolPoint(PatrolPoint _p)
    {
        int currentIdx = _p.IndexInArray;
        if(Random.Range(0.0f, 1.0f) > 0.3f)
        {
            currentIdx += 1;
        }
        else
        {
            currentIdx -= 1;    
        }
        
        if (currentIdx >= PatrolPoints.Length)
        {
            currentIdx = 0;
        }

        if(currentIdx < 0)
        {
            currentIdx = PatrolPoints.Length - 1;
        }
        Debug.Log("Choosing Index" + currentIdx);
        return PatrolPoints[currentIdx];
    }

    public PatrolPoint GetClosestPatrolPoint(GameObject _obj)
    {
        float minimDistance = Mathf.Infinity;
        PatrolPoint closest = null;
        for (int i = 0; i < PatrolPoints.Length; ++i)
        {
            PatrolPoint tmp = PatrolPoints[i];
            float currentDistance = Vector3.Distance(_obj.transform.position, tmp.transform.position);
            if(currentDistance < minimDistance)
            {
                minimDistance = currentDistance;
                closest = tmp;
            }
        }
        return closest;
    }
}
