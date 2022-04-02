using UnityEngine;

public class VampireMovement : MonoBehaviour
{

    private Rigidbody m_RigidBody;
    private PlayerStats m_PlayerStats;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

    private void Awake()
    {
        m_PlayerStats = GetComponent<PlayerStats>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GameController.OnNightStarted += GameController_OnNightStarted;
    }

    private void OnDisable()
    {
        GameController.OnNightStarted -= GameController_OnNightStarted;
    }

    private void GameController_OnNightStarted()
    {
        ResetPosAndRot();
    }

    private void Start()
    {
        m_OriginalPosition = transform.position;
        m_OriginalRotation = transform.rotation;
    }


    public void ResetPosAndRot()
    {
        transform.position = m_OriginalPosition;
        transform.rotation = m_OriginalRotation;
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.GameState != GameState.NIGHT)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, m_OriginalPosition.y + Mathf.Sin(Time.time) * 0.3f, transform.position.z);

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        m_RigidBody.velocity = (-transform.right * vertical) * m_PlayerStats.Speed * Time.fixedDeltaTime;
        transform.Rotate((transform.up * horizontal) * 200 * Time.fixedDeltaTime);
    }
}
