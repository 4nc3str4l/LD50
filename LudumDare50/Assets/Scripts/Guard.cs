using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public District OwnerDistict;

    private PatrolPoint TargetPatrolPoint;
    private List<PatrolPoint> m_LoadedPath = new List<PatrolPoint>();

    private Rigidbody m_RigidBody;
    public float WalkingSpeed;

    public float m_TimeUntilUnalert = 0;
    public float m_AlertTime = 0;

    private enum State
    {
        PATROL,
        CHASING
    }

    private State m_CurrentState;

    public void Init(District _spawner, PatrolPoint _p)
    {
        m_RigidBody = GetComponent<Rigidbody>();
        OwnerDistict = _spawner;
        TargetPatrolPoint = _p;
        WalkingSpeed = 300f + Random.Range(30, 60);
        m_CurrentState = State.PATROL;

        OwnerDistict.OnGoToBuldingRequired += OwnerDistict_OnGoToBuildingRequired;
    }

    private void OwnerDistict_OnGoToBuildingRequired(int buldingIdx)
    {
        Alert();
        // If we are already going to the target just ignore the event
        if(TargetPatrolPoint.IndexInArray == buldingIdx || (m_LoadedPath.Count > 0 &&  m_LoadedPath[m_LoadedPath.Count - 1].IndexInArray == buldingIdx))
        {
            return;
        }
        RecomputePath(buldingIdx);
    }

    private void OnDestroy()
    {
        OwnerDistict.OnGoToBuldingRequired -= OwnerDistict_OnGoToBuildingRequired;
    }

    private void Update()
    {

        float baseSpeed = !IsAlerted() ? WalkingSpeed : WalkingSpeed * 2;

        switch (m_CurrentState)
        {
            case State.CHASING:
                UpdateChase(baseSpeed); 
                break;
            case State.PATROL:
                UpdatePatrol(baseSpeed);
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        GameController.OnDayStarted += GameController_OnDayStarted;
    }

    private void OnDisable()
    {
        GameController.OnDayStarted -= GameController_OnDayStarted;
    }

    public void SetPatrol()
    {
        m_CurrentState = State.PATROL;
        TargetPatrolPoint = OwnerDistict.GetClosestPatrolPoint(gameObject);
    }

    public void SetChasing()
    {
        m_CurrentState = State.CHASING;
    }

    private void UpdatePatrol(float _baseSpeed)
    {
        if(ReachedTarget(TargetPatrolPoint.transform.position, 1.5f))
        {
            if (m_LoadedPath.Count == 0)
            {
                m_LoadedPath = OwnerDistict.GetShortestPathTo(TargetPatrolPoint.IndexInArray, OwnerDistict.GetRandomIndex());
            }
            else
            {
                TargetPatrolPoint = m_LoadedPath[0];
                m_LoadedPath.RemoveAt(0);
            }
        }
        MoveTowardsTarget(TargetPatrolPoint.transform.position, _baseSpeed);
    }

    private void RecomputePath(int _target)
    {
        m_LoadedPath = OwnerDistict.GetShortestPathTo(TargetPatrolPoint.IndexInArray, OwnerDistict.GetRandomIndex());
        TargetPatrolPoint = m_LoadedPath[0];
        m_LoadedPath.RemoveAt(0);
    }

    private void UpdateChase(float _baseSpeed)
    {
        MoveTowardsTarget(PlayerStats.Instance.transform.position, _baseSpeed * 3);
        if(ReachedTarget(PlayerStats.Instance.transform.position, 3.5f, false))
        {
            GameController.Instance.PlayerKilled();
        }
    }

    private void MoveTowardsTarget(Vector3 _t, float _speed)
    {
        transform.LookAt(_t);
        Vector3 direction = (_t - transform.position).normalized;
        m_RigidBody.velocity = direction * _speed * Time.deltaTime;
    }

    private bool ReachedTarget(Vector3 _target, float _tolerance, bool _debug = false)
    {
        float distanceToTarget = Vector2.Distance(GetVec2d(_target), GetVec2d(transform.position));
        if(_debug)
        {
            Debug.Log("Distance To Player " + distanceToTarget);
        }
        return distanceToTarget <= _tolerance;
    }

    private void GameController_OnDayStarted()
    {
        Destroy(gameObject);
    }

    private Vector2 GetVec2d(Vector3 _v)
    {
        return new Vector2(_v.x, _v.z);
    }


    private void Alert()
    {
        m_AlertTime += 5;
        m_TimeUntilUnalert = Time.time + m_AlertTime;
    }

    private bool IsAlerted()
    {
        return m_TimeUntilUnalert >= Time.time;
    }
}
