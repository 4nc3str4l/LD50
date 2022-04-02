using UnityEngine;

public class VampireMovement : MonoBehaviour
{

    private Rigidbody m_RigidBody;
    private PlayerStats m_PlayerStats;

    private void Awake()
    {
        m_PlayerStats = GetComponent<PlayerStats>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        m_RigidBody.velocity = (-transform.right * vertical) * m_PlayerStats.Speed * Time.fixedDeltaTime;
        transform.Rotate((transform.up * horizontal) * 100 * Time.fixedDeltaTime);
    }
}
