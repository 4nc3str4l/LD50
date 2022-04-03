using UnityEngine;

public class VampireMovement : MonoBehaviour
{

    private Rigidbody m_RigidBody;
    private PlayerStats m_PlayerStats;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

    private bool m_IsMoving = false;

    public float FootStepRate = 0.2f;
    private float m_NextFootStep = 0;


    private void Awake()
    {
        m_PlayerStats = GetComponent<PlayerStats>();
        m_RigidBody = GetComponent<Rigidbody>();

        m_OriginalPosition = transform.position;
        m_OriginalRotation = transform.rotation;
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

        m_IsMoving = vertical != 0 || horizontal != 0;
    }

    private void Update()
    {
        if (m_IsMoving)
        {
            if(Time.time > m_NextFootStep)
            {
                Jukebox.Instance.PlaySound(Jukebox.Instance.FootStep, 0.075f);
                m_NextFootStep = Time.time + FootStepRate;
            }
        }
    }
}
