using UnityEngine;

public class MazeClose : MonoBehaviour
{
    public static MazeClose Instance { get; private set; }

    public MazePoint[] nodes;

    void Awake()
    {
        Instance = this;
        nodes = FindObjectsOfType<MazePoint>();
    }

    public MazePoint GetClosestNode(Vector3 position)
    {
        MazePoint best = null;
        float bestDist = float.MaxValue;

        foreach (var n in nodes)
        {
            float d = (n.transform.position - position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = n;
            }
        }
        return best;
    }
}
