using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Transform))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private NavMeshAgent agent;

    [Tooltip("0.01 ist tatsaechlich ein ziemlich guter Wert")]
    [SerializeField]
    public float smoothSpeed = 0.01f;
    [SerializeField]
    public float CameraOffsetX = 0.0f;
    [SerializeField]
    public float CameraOffsetY = 3.0f;
    [SerializeField]
    public float CameraOffsetZ = -6.0f;

    [SerializeField]
    private Transform m_borderLeft;
    [SerializeField]
    private Transform m_borderRight;


    private Vector3 m_CameraOffset;
    private Vector3 m_LastPos;


    void Start()
    {
        agent = target.GetComponent<NavMeshAgent>();
        m_CameraOffset = new Vector3(CameraOffsetX, CameraOffsetY, CameraOffsetZ);
        m_LastPos = transform.position;
        m_CameraOffset.x = CameraOffsetX;
        m_CameraOffset.y = CameraOffsetY;
        m_CameraOffset.z = CameraOffsetZ;
        //transform.position = target.position + m_CameraOffset;
    }

    private void LateUpdate() {

        Vector3 desiredPosition = target.position + m_CameraOffset;
        Vector3 currentVelocity;
        if (IsNewXPositionBad(ref desiredPosition.x))
            // This fixes jittering, when the camera is leaving the specified area
            currentVelocity = transform.position - m_LastPos;
        else
            // This fixes jitting, when the camera is within the specified area
            currentVelocity = agent.velocity;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
        m_LastPos = transform.position;
    }


    /// <summary>
    /// Checks if the new x position is outside of the specified area
    /// </summary>
    /// <param name="positionX"> desired x position</param>
    /// <returns>true: if outside, false if not outside</returns>
    private bool IsNewXPositionBad(ref float positionX) {
        if (positionX < m_borderLeft.position.x) {
            positionX = m_LastPos.x;
            return true;
        } else if (positionX > m_borderRight.position.x) {
            positionX = m_LastPos.x;
            return true;
        }
        return false;
    }

    public void ResetCamera()
    {
        transform.position = target.position + m_CameraOffset;
    }
}
