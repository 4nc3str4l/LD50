using UnityEngine;

public class Guard : MonoBehaviour
{
    public District OwnerDistict;
    private PatrolPoint TargetPatrolPoint;
    private Rigidbody m_RigidBody;

    public float WalkingSpeed;


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
    }

    private void Update()
    {
        switch (m_CurrentState)
        {
            case State.CHASING:
                UpdateChase(); 
                break;
            case State.PATROL:
                UpdatePatrol();
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

    private void UpdatePatrol()
    {
        if(ReachedTarget(TargetPatrolPoint.transform.position, 1f))
        {
            TargetPatrolPoint = OwnerDistict.ChooseNextPatrolPoint(TargetPatrolPoint);
        }
        MoveTowardsTarget(TargetPatrolPoint.transform.position, WalkingSpeed);
    }

    private void UpdateChase()
    {
        MoveTowardsTarget(PlayerStats.Instance.transform.position, WalkingSpeed * 3);
        if(ReachedTarget(PlayerStats.Instance.transform.position, 2.5f))
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

    private bool ReachedTarget(Vector3 _target, float _tolerance)
    {
        float distanceToTarget = Vector2.Distance(GetVec2d(_target), GetVec2d(transform.position));
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
}
