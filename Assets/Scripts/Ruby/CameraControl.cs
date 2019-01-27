using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
    public Transform[] m_Targets; // All the targets the camera needs to encompass.


    new Transform transform;
    private Camera m_Camera;
    private Transform m_camHolder;
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                
    private Vector3 m_DesiredPosition;             


    private void Awake()
    {
        if (transform == null) transform = GetComponent<Transform>();
        m_Camera = GetComponentInChildren<Camera>();
        m_camHolder = transform.parent.transform;
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();
        m_camHolder.position = Vector3.SmoothDamp(m_camHolder.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf) continue;
            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0) averagePos /= numTargets;
        averagePos.z = m_camHolder.position.z;

        // The desired position is the average position;
        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = m_camHolder.InverseTransformPoint(m_DesiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf) continue;

            // Find the position of the target in the camera's local space.
            Vector3 targetLocalPos = m_camHolder.InverseTransformPoint(m_Targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        // Add the edge buffer to the size.
        size += m_ScreenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, m_MinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        m_camHolder.position = m_DesiredPosition;

        // Find and set the required size of the camera.
        m_Camera.orthographicSize = FindRequiredSize();
    }
}