using System.Collections.Generic;
using UnityEngine;

public class MazePoint : MonoBehaviour
{
    public List<MazePoint> neighbors = new List<MazePoint>();

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.cyan;
        foreach (var n in neighbors)
        {
            if (n != null)
                Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }
}
