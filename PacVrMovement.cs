using UnityEngine;

public class PacVRMovement : MonoBehaviour
{
    public Transform head;          // Camera transform
    public float speed = 3f;
    public float checkDistance = 0.6f;
    public LayerMask wallMask;
    public float cellSize = 1f;     // distance between corridor center lines

    Vector3 currentDir = Vector3.zero;

    void Start()
    {
        if (head == null && Camera.main != null)
            head = Camera.main.transform;
    }

    void Update()
    {
        if (head == null) return;

        // Take head forward, project to XZ plane
        Vector3 fwd = head.forward;
        fwd.y = 0f;
        if (fwd.sqrMagnitude < 0.0001f) return;
        fwd.Normalize();

        // Quantize to one of four directions
        Vector3 desiredDir;
        if (Mathf.Abs(fwd.x) > Mathf.Abs(fwd.z))
            desiredDir = (fwd.x > 0f) ? Vector3.right : Vector3.left;
        else
            desiredDir = (fwd.z > 0f) ? Vector3.forward : Vector3.back;

        // Try to turn into desired direction when possible
        if (CanMove(desiredDir))
            currentDir = desiredDir;

        // Move in current direction if free
        if (CanMove(currentDir))
            transform.position += currentDir * speed * Time.deltaTime;

        SnapToCenterLines();
    }

    bool CanMove(Vector3 dir)
    {
        if (dir == Vector3.zero) return false;
        Ray ray = new Ray(transform.position, dir);
        return !Physics.Raycast(ray, checkDistance, wallMask, QueryTriggerInteraction.Ignore);
    }

    void SnapToCenterLines()
    {
        Vector3 p = transform.position;

        if (Mathf.Abs(currentDir.x) > 0.1f)
        {
            // Moving east or west: lock Z to nearest lane
            p.z = Mathf.Round(p.z / cellSize) * cellSize;
        }
        else if (Mathf.Abs(currentDir.z) > 0.1f)
        {
            // Moving north or south: lock X to nearest lane
            p.x = Mathf.Round(p.x / cellSize) * cellSize;
        }

        transform.position = p;
    }
}
