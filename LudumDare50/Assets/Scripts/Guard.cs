using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Guard : MonoBehaviour
{
    public District OwnerDistict;

    private PatrolPoint TargetPatrolPoint;
    private List<PatrolPoint> m_LoadedPath = new List<PatrolPoint>();

    private Rigidbody m_RigidBody;
    public float WalkingSpeed;

    public float m_TimeUntilUnalert = 0;
    public float m_AlertTime = 0;

    public GuardSensor m_Sensors;

    private State m_CurrentState;

    public const float TIME_TO_SPONTANEOUS_CHANGE = 60f;
    public float m_SpontaneusWayChange = 0;

    public enum State
    {
        PATROL,
        CHASING,
        DEAD,
        STUNNED,
    }

    private void Awake()
    {
        m_Sensors = GetComponentInChildren<GuardSensor>();
    }


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

        float baseSpeed = !IsAlerted() ? WalkingSpeed : WalkingSpeed * 1.1f;

        switch (m_CurrentState)
        {
            case State.CHASING:
                UpdateChase(baseSpeed); 
                break;
            case State.PATROL:
                UpdatePatrol(baseSpeed);
                break;
            case State.DEAD:
                break;
            case State.STUNNED:
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        GameController.OnDayStarted += GameController_OnDayStarted;
        InvisibilitySkill.OnInvisibilityStarted += InvisibilitySkill_OnInvisibilityStarted;
        InvisibilitySkill.OnInvisibilityEnded += InvisibilitySkill_OnInvisibilityEnded;
    }

    private void OnDisable()
    {
        GameController.OnDayStarted -= GameController_OnDayStarted;
        InvisibilitySkill.OnInvisibilityStarted -= InvisibilitySkill_OnInvisibilityStarted;
        InvisibilitySkill.OnInvisibilityEnded -= InvisibilitySkill_OnInvisibilityEnded;
    }

    private void InvisibilitySkill_OnInvisibilityEnded()
    {
        m_Sensors.gameObject.SetActive(true);
    }

    private void InvisibilitySkill_OnInvisibilityStarted()
    {
        m_Sensors.gameObject.SetActive(false);
        SetPatrol();
    }

    public void SetPatrol()
    {
        m_CurrentState = State.PATROL;
        TargetPatrolPoint = OwnerDistict.GetClosestPatrolPoint(gameObject);
        m_SpontaneusWayChange = Time.time + TIME_TO_SPONTANEOUS_CHANGE;
    }

    void SpontaneousDesicion()
    {
        if(m_SpontaneusWayChange <= Time.time)
        {
            RecomputePath(OwnerDistict.GetRandomIndex());
            m_SpontaneusWayChange = Time.time + TIME_TO_SPONTANEOUS_CHANGE + Random.Range(-10f, 10f);
        }
    }

    public void SetChasing()
    {
        m_CurrentState = State.CHASING;
        Jukebox.Instance.PlaySound(Jukebox.Instance.HeyYou, 0.6f);
    }

    public void SetDead()
    {
        m_CurrentState = State.DEAD;
    }

    public void Stun(float _seconds)
    {
        State lastState = m_CurrentState;
        m_CurrentState = State.STUNNED;
        m_Sensors.gameObject.SetActive(false);
        m_RigidBody.velocity = Vector3.zero;

        Vector3 originalScale = transform.localScale;
        transform.DOShakeScale(_seconds / 2).OnComplete(()=>
        {
            if(transform != null)
            {
                transform.localScale = originalScale;
            }
        });

        transform.DOShakePosition(_seconds).OnComplete(() => {
            if (m_Sensors != null)
            {
                if (m_CurrentState != State.DEAD)
                {
                    m_Sensors.gameObject.SetActive(true);
                    m_CurrentState = lastState;
                }
            }
        });
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

        SpontaneousDesicion();
    }


    private void RecomputePath(int _target)
    {
        m_LoadedPath = OwnerDistict.GetShortestPathTo(TargetPatrolPoint.IndexInArray, _target);
        TargetPatrolPoint = m_LoadedPath[0];
        m_LoadedPath.RemoveAt(0);
    }

    private void UpdateChase(float _baseSpeed)
    {

        if(ReachedTarget(PlayerStats.Instance.transform.position, 3.5f, false))
        {
            GameController.Instance.PlayerKilled();
            m_RigidBody.velocity = Vector3.zero;
        }
        else
        {
            MoveTowardsTarget(PlayerStats.Instance.transform.position, Mathf.Min(_baseSpeed * 2, PlayerStats.Instance.Speed * 1.05f));
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
        m_AlertTime = 10;
        m_TimeUntilUnalert = Time.time + m_AlertTime;
    }

    private bool IsAlerted()
    {
        return m_TimeUntilUnalert >= Time.time;
    }

    public State CurrentState()
    {
        return m_CurrentState;
    }
}
