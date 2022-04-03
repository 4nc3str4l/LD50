using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    private float m_Speed = 2f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_Speed);
        transform.position = smoothedPosition;
    }
}
