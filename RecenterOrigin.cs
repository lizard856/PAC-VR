using UnityEngine;

public class RecenterOrigin : MonoBehaviour
{
    public Transform head;    // Main Camera
    public Transform anchor;  // where you want the head to be (Start point)

    public void Recenter()
    {
        if (head == null) head = Camera.main != null ? Camera.main.transform : null;
        if (head == null || anchor == null) return;

        // 1. Position: move XR Origin so head ends up at anchor position (horizontal only)
        Vector3 offset = anchor.position - head.position;
        offset.y = 0f;  // keep floor / height as is
        transform.position += offset;

        // 2. Rotation: align head yaw to anchor yaw
        float currentYaw = head.eulerAngles.y;
        float targetYaw  = anchor.eulerAngles.y;
        float deltaYaw   = targetYaw - currentYaw;

        transform.Rotate(Vector3.up, deltaYaw, Space.World);
    }

    void Start()
    {
        // auto recenter when scene starts
        Recenter();
    }
}
