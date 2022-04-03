using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class District : MonoBehaviour
{
    public int DistrictNumber;
    public int NumWatchersToSpawn = 0;
    public int KillCounter = 0;
    public int GuardKillCounter = 0;

    public TMP_Text DistrictText;

    public PatrolPoint[] PatrolPoints;

    public GameObject GuardPrefab;

    public event Action<int> OnGoToBuldingRequired;

    public static List<District> AttackedDistrics;
    public List<Bulding> DistrictBuildings = new List<Bulding>();

    private void Awake()
    {
        AttackedDistrics = new List<District>();
        DistrictText.text = "District " + DistrictNumber;

        for (int i = 0; i < PatrolPoints.Length; ++i)
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
        if(DistrictNumber == 1)
        {
            AttackedDistrics.Clear();
        }

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

        if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.7f)
        {
            NumWatchersToSpawn += 1;
        }
    }

    public void SpawnGuard()
    {
        GameObject g = GameObject.Instantiate(GuardPrefab);
        PatrolPoint p = PatrolPoints[UnityEngine.Random.Range(0, PatrolPoints.Length)];
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
        if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.3f)
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

    public List<PatrolPoint> GetShortestPathTo(int _current, int _patrolIndex)
    {
        if(_current == _patrolIndex)
        {
            return new List<PatrolPoint> { PatrolPoints[_current] };
        }

        List<PatrolPoint> p1 = new List<PatrolPoint>();
        int pIdx = _current;
        while(pIdx != _patrolIndex)
        {
            pIdx += 1;
            if(pIdx >= PatrolPoints.Length)
            {
                pIdx = 0;
            }
            p1.Add(PatrolPoints[pIdx]);
        }

        pIdx = _current;
        List<PatrolPoint> p2 = new List<PatrolPoint>();
        while (pIdx != _patrolIndex)
        {
            pIdx -= 1;
            if (pIdx < 0)
            {
                pIdx = PatrolPoints.Length -1;
            }
            p2.Add(PatrolPoints[pIdx]);
        }
        return p2.Count > p1.Count ? p1 : p2;
    }

    public void BuldingUnderAttack(int _idx)
    {
        OnGoToBuldingRequired?.Invoke(_idx);
    }

    public int GetRandomIndex()
    {
        return UnityEngine.Random.Range(0, PatrolPoints.Length);
    }

    public static void DeclareAttackedDistrict(District _district)
    {
        if(AttackedDistrics.Contains(_district))
        {
            return;
        }

        AttackedDistrics.Add(_district);
    }

    public string GetSecurityReport()
    {
        int numAlarms = 0;
        foreach(Bulding b in DistrictBuildings)
        {
            if (b.Alarm)
            {
                numAlarms += 1;
            }
        }
        string districtReport = "District " + DistrictNumber +  ": ";
        if (numAlarms == 0 && NumWatchersToSpawn == 0)
        {
            return districtReport + " (Nothing For Now, our population feels safe)";
        }
        else if(numAlarms > 0 && NumWatchersToSpawn == 0)
        {
            return districtReport + numAlarms + " home have alarms installed (no private security for now)";
        }
        else if(numAlarms == 0 && NumWatchersToSpawn > 0)
        {
            return districtReport + NumWatchersToSpawn + " where hired for tonight to improve security as we have been attacked";
        }
        else
        {
            return districtReport + " The situation in is bad, now we have " + numAlarms + " homes with alarms and " + NumWatchersToSpawn + " guards hired";
        }
    }
}
